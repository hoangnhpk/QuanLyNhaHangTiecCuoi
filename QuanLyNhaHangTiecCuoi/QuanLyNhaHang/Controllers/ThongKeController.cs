using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cần cái này để query async nếu muốn
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QuanLyNhaHang.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public ThongKeController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStatData(string type, DateTime? date, int page = 1)
        {
            var selectedDate = date ?? DateTime.Now;
            var query = _context.DatTiecs.AsQueryable();
            int pageSize = 5; // Số dòng mỗi trang

            // 1. LỌC THỜI GIAN
            if (type == "day")
            {
                query = query.Where(x => x.NgayToChuc.HasValue && x.NgayToChuc.Value.Date == selectedDate.Date);
            }
            else if (type == "month")
            {
                query = query.Where(x => x.NgayToChuc.HasValue && x.NgayToChuc.Value.Month == selectedDate.Month && x.NgayToChuc.Value.Year == selectedDate.Year);
            }
            else // year
            {
                query = query.Where(x => x.NgayToChuc.HasValue && x.NgayToChuc.Value.Year == selectedDate.Year);
            }

            // Lấy dữ liệu thô về RAM để xử lý logic phức tạp (Status check)
            // Lưu ý: Với dữ liệu cực lớn thì nên xử lý SQL Raw, nhưng với App quản lý nhà hàng thì OK.
            var rawData = query.Include(x => x.KhachHang).ToList();

            // 2. XỬ LÝ SỐ LIỆU TỔNG HỢP (KPIs)
            decimal totalRevenue_Expected = 0; // Dự kiến
            decimal totalRevenue_Actual = 0;   // Thực thu
            decimal totalRemaining = 0;        // Còn lại
            int countCoc = 0;
            int countDone = 0;
            int countCancel = 0;

            foreach (var item in rawData)
            {
                decimal giaTriTiec = (item.GiaBan ?? 0) * (item.SoBanDat ?? 0);
                decimal tienCoc = item.TienCoc ?? 0;
                string status = (item.TrangThai ?? "").ToLower();

                bool isCancelled = status.Contains("huy") || status.Contains("cancel");
                bool isCompleted = status.Contains("hoan thanh") || status.Contains("thanh toan du") || status.Contains("completed");

                if (isCancelled)
                {
                    countCancel++;
                    // Hủy thì thực thu = cọc (mất cọc) hoặc hoàn cọc tùy chính sách. 
                    // Giả sử: Hủy là mất cọc -> Doanh thu là tiền cọc.
                    totalRevenue_Actual += tienCoc;
                    // Doanh thu dự kiến của đơn hủy coi như = 0 hoặc bằng tiền cọc (tùy nghiệp vụ). Ở đây tính là tiền cọc.
                    totalRevenue_Expected += tienCoc;
                }
                else
                {
                    totalRevenue_Expected += giaTriTiec;

                    if (isCompleted)
                    {
                        countDone++;
                        totalRevenue_Actual += giaTriTiec;
                    }
                    else
                    {
                        countCoc++; // Coi như đang ở trạng thái cọc/chưa xong
                        totalRevenue_Actual += tienCoc;
                        totalRemaining += (giaTriTiec - tienCoc);
                    }
                }
            }

            // 3. XỬ LÝ BIỂU ĐỒ (Chart)
            List<ChartDataPoint> chartData = new List<ChartDataPoint>();
            if (type == "month")
            {
                // Chia 4 tuần
                chartData = GenerateWeeklyChartV2(rawData);
            }
            else if (type == "year")
            {
                // Chia 12 tháng
                chartData = GenerateMonthlyChartV2(rawData);
            }
            else
            {
                // Theo ngày (hiện 1 cột duy nhất hoặc chia theo giờ nếu muốn)
                chartData.Add(new ChartDataPoint
                {
                    Label = selectedDate.ToString("dd/MM"),
                    TotalCount = rawData.Count,
                    CancelCount = countCancel,
                    Percentage = 100
                });
            }

            // 4. XỬ LÝ DANH SÁCH ĐƠN HÀNG (Phân trang)
            // Sắp xếp ngày mới nhất
            var sortedList = rawData.OrderByDescending(x => x.NgayToChuc).ToList();
            int totalItems = sortedList.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedData = sortedList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var orderList = paginatedData.Select(x => {
                decimal val = (x.GiaBan ?? 0) * (x.SoBanDat ?? 0);
                string st = (x.TrangThai ?? "").ToLower();
                bool isCan = st.Contains("huy");
                bool isComp = st.Contains("hoan thanh") || st.Contains("completed");

                string paymentStatus = isCan ? "Đã hủy" : (isComp ? "Thanh toán 100%" : "Cọc 30%");

                return new OrderItem
                {
                    MaDatTiec = x.MaDatTiec,
                    NgayToChuc = x.NgayToChuc?.ToString("dd/MM/yyyy") ?? "",
                    KhachHang = x.KhachHang?.TenKhachHang ?? "Khách lẻ",
                    TongTien = FormatCurrency(val),
                    TrangThai = x.TrangThai,
                    TinhTrangThanhToan = paymentStatus,
                    IsCancelled = isCan
                };
            }).ToList();

            // 5. ĐÓNG GÓI
            var model = new ThongKeViewModel
            {
                TongHop = new ThongKeTongHop
                {
                    TongSoTiec = rawData.Count,
                    SoTiecDaHuy = countCancel,
                    DoanhThuDuKien = FormatCurrency(totalRevenue_Expected),
                    ThucThu = FormatCurrency(totalRevenue_Actual),
                    ConLai = FormatCurrency(totalRemaining),
                    SoDonCoc = countCoc,
                    SoDonHoanTat = countDone
                },
                Bieudo = chartData,
                DanhSachDonHang = new OrderListResult
                {
                    Items = orderList,
                    CurrentPage = page,
                    TotalPages = totalPages
                }
            };

            return Json(model);
        }

        // --- HELPERS ---
        private string FormatCurrency(decimal value)
        {
            return string.Format(new CultureInfo("vi-VN"), "{0:C0}", value);
        }

        private List<ChartDataPoint> GenerateWeeklyChartV2(List<DatTiec> data)
        {
            var result = new List<ChartDataPoint>();
            var weeks = new[] {
                new { L = "Tuần 1", S = 1, E = 7 },
                new { L = "Tuần 2", S = 8, E = 14 },
                new { L = "Tuần 3", S = 15, E = 21 },
                new { L = "Tuần 4", S = 22, E = 31 }
            };

            int maxVal = 0;
            foreach (var w in weeks)
            {
                var subset = data.Where(x => x.NgayToChuc.Value.Day >= w.S && x.NgayToChuc.Value.Day <= w.E).ToList();
                int total = subset.Count;
                int cancel = subset.Count(x => (x.TrangThai ?? "").ToLower().Contains("huy"));
                if (total > maxVal) maxVal = total;

                result.Add(new ChartDataPoint { Label = w.L, TotalCount = total, CancelCount = cancel });
            }

            // Tính %
            foreach (var r in result) r.Percentage = maxVal > 0 ? ((double)r.TotalCount / maxVal) * 100 : 0;
            return result;
        }

        private List<ChartDataPoint> GenerateMonthlyChartV2(List<DatTiec> data)
        {
            var result = new List<ChartDataPoint>();
            int maxVal = 0;
            for (int i = 1; i <= 12; i++)
            {
                var subset = data.Where(x => x.NgayToChuc.Value.Month == i).ToList();
                int total = subset.Count;
                int cancel = subset.Count(x => (x.TrangThai ?? "").ToLower().Contains("huy"));
                if (total > maxVal) maxVal = total;

                result.Add(new ChartDataPoint { Label = "T" + i, TotalCount = total, CancelCount = cancel });
            }
            foreach (var r in result) r.Percentage = maxVal > 0 ? ((double)r.TotalCount / maxVal) * 100 : 0;
            return result;
        }
        [HttpGet]
        public IActionResult GetOrderDetail(string id)
        {
            var order = _context.DatTiecs
                .Include(x => x.KhachHang)
                .Include(x => x.PhieuThanhToan)
                // Include bảng Món ăn
                .Include(x => x.ChiTietThucDons).ThenInclude(m => m.MonAn)
                // Include bảng Dịch vụ
                .Include(x => x.TT_SuDungDichVus).ThenInclude(d => d.DichVu)
                .FirstOrDefault(x => x.MaDatTiec == id);

            if (order == null) return NotFound();

            var result = new
            {
                MaDatTiec = order.MaDatTiec,
                NgayDat = order.NgayDatTiec?.ToString("dd/MM/yyyy"),
                NgayToChuc = order.NgayToChuc?.ToString("dd/MM/yyyy"),
                GioToChuc = order.GioToChuc?.ToString(@"hh\:mm"),
                SoBan = order.SoBanDat,
                TenKhach = order.KhachHang?.TenKhachHang ?? "Khách lẻ",
                Sdt = order.KhachHang?.SdtKhachHang ?? "---",
                GhiChu = order.ChiTiet ?? "Không có ghi chú",

                // Tài chính
                TongTien = string.Format(new CultureInfo("vi-VN"), "{0:C0}", (order.GiaBan ?? 0) * (order.SoBanDat ?? 0)),
                TienCoc = string.Format(new CultureInfo("vi-VN"), "{0:C0}", order.TienCoc ?? 0),
                TrangThai = order.TrangThai,

                // --- SỬA LỖI TẠI ĐÂY ---

                // 1. Danh sách món (Lấy từ bảng MON_AN - DonGia)
                MonAn = order.ChiTietThucDons.Select(m => new {
                    TenMon = m.MonAn.TenMonAn,
                    SoLuong = m.SoLuongMotBan,
                    // Nếu bảng chi tiết (m) không có giá, lấy từ bảng gốc (m.MonAn)
                    DonGia = string.Format(new CultureInfo("vi-VN"), "{0:C0}", m.MonAn.DonGia ?? 0)
                }).ToList(),

                // 2. Danh sách dịch vụ (Lấy từ bảng DICH_VU - GiaDV)
                DichVu = order.TT_SuDungDichVus.Select(d => new {
                    TenDV = d.DichVu.TenDichVu,
                    SoLuong = d.SoLuong,
                    // SỬA: Dùng GiaDV thay vì DonGia
                    DonGia = string.Format(new CultureInfo("vi-VN"), "{0:C0}", d.DichVu.GiaDV ?? 0)
                }).ToList()
            };

            return Json(result);
        }
    }
}