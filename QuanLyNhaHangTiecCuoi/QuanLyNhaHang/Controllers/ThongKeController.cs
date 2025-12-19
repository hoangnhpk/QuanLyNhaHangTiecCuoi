using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult GetStatData(string type, DateTime? date, string statusFilter = "Tất cả", int page = 1)
        {
            var selectedDate = date ?? DateTime.Now;
            var query = _context.DatTiecs.AsQueryable();
            int pageSize = 5;

            // 1. LỌC THỜI GIAN: Kiểm tra cả Ngày Đặt và Ngày Tổ Chức
            if (type == "day")
            {
                var d = selectedDate.Date;
                query = query.Where(x => (x.NgayToChuc.HasValue && x.NgayToChuc.Value.Date == d)
                                      || (x.NgayDatTiec.HasValue && x.NgayDatTiec.Value.Date == d));
            }
            else if (type == "month")
            {
                int m = selectedDate.Month;
                int y = selectedDate.Year;
                query = query.Where(x => (x.NgayToChuc.HasValue && x.NgayToChuc.Value.Month == m && x.NgayToChuc.Value.Year == y)
                                      || (x.NgayDatTiec.HasValue && x.NgayDatTiec.Value.Month == m && x.NgayDatTiec.Value.Year == y));
            }
            else // year
            {
                int y = selectedDate.Year;
                query = query.Where(x => (x.NgayToChuc.HasValue && x.NgayToChuc.Value.Year == y)
                                      || (x.NgayDatTiec.HasValue && x.NgayDatTiec.Value.Year == y));
            }

            // 2. LỌC THEO TRẠNG THÁI
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "Tất cả")
            {
                if (statusFilter == "Đã hủy") query = query.Where(x => x.TrangThai == "huy");
                else if (statusFilter == "Hoàn tất") query = query.Where(x => x.TrangThai == "Completed");
                else query = query.Where(x => x.TrangThai != "huy" && x.TrangThai != "Completed"); // Đã cọc
            }

            var rawData = query.Include(x => x.KhachHang).ToList();

            // 3. XỬ LÝ SỐ LIỆU TỔNG HỢP (KPIs)
            decimal totalRevenue_Expected = 0;
            decimal totalRevenue_Actual = 0;
            decimal totalRemaining = 0;
            int countCoc = 0;
            int countDone = 0;
            int countCancel = 0;

            foreach (var item in rawData)
            {
                decimal giaTriTiec = (item.GiaBan ?? 0) * (item.SoBanDat ?? 0);
                decimal tienCoc = item.TienCoc ?? 0;
                string st = (item.TrangThai ?? "").ToLower();

                if (st == "huy")
                {
                    countCancel++;
                    totalRevenue_Actual += tienCoc;
                    totalRevenue_Expected += tienCoc;
                }
                else
                {
                    totalRevenue_Expected += giaTriTiec;
                    if (st == "completed")
                    {
                        countDone++;
                        totalRevenue_Actual += giaTriTiec;
                    }
                    else
                    {
                        countCoc++;
                        totalRevenue_Actual += tienCoc;
                        totalRemaining += (giaTriTiec - tienCoc);
                    }
                }
            }

            // 4. BIỂU ĐỒ & PHÂN TRANG
            var sortedList = rawData.OrderByDescending(x => x.NgayToChuc).ToList();
            int totalItems = sortedList.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedData = sortedList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var orderList = paginatedData.Select(x => new OrderItem
            {
                MaDatTiec = x.MaDatTiec,
                NgayToChuc = x.NgayToChuc?.ToString("dd/MM/yyyy") ?? "",
                KhachHang = x.KhachHang?.TenKhachHang ?? "Khách lẻ",
                TongTien = FormatCurrency((x.TrangThai == "huy") ? (x.TienCoc ?? 0) : (x.GiaBan ?? 0) * (x.SoBanDat ?? 0)),
                TrangThai = x.TrangThai,
                TinhTrangThanhToan = (x.TrangThai == "huy") ? "Đã hủy" : (x.TrangThai == "Completed" ? "Thanh toán 100%" : "Cọc 30%"),
                IsCancelled = (x.TrangThai == "huy")
            }).ToList();

            return Json(new ThongKeViewModel
            {
                TongHop = new ThongKeTongHop
                {
                    TongSoTiec = rawData.Count,
                    SoTiecDaHuy = countCancel,
                    DoanhThuDuKien = FormatCurrency(totalRevenue_Expected),
                    ThucThu = FormatCurrency(totalRevenue_Actual),
                    ConLai = FormatCurrency(totalRemaining)
                },
                Bieudo = (type == "month") ? GenerateWeeklyChartV2(rawData) : GenerateMonthlyChartV2(rawData),
                DanhSachDonHang = new OrderListResult { Items = orderList, CurrentPage = page, TotalPages = totalPages }
            });
        }

        private string FormatCurrency(decimal val) => string.Format(new CultureInfo("vi-VN"), "{0:C0}", val);

        private List<ChartDataPoint> GenerateWeeklyChartV2(List<DatTiec> data)
        {
            var res = new List<ChartDataPoint>();
            var weeks = new[] { new { L = "Tuần 1", S = 1, E = 7 }, new { L = "Tuần 2", S = 8, E = 14 }, new { L = "Tuần 3", S = 15, E = 21 }, new { L = "Tuần 4", S = 22, E = 31 } };
            int max = 0;
            foreach (var w in weeks)
            {
                var sub = data.Where(x => x.NgayToChuc.HasValue && x.NgayToChuc.Value.Day >= w.S && x.NgayToChuc.Value.Day <= w.E).ToList();
                if (sub.Count > max) max = sub.Count;
                res.Add(new ChartDataPoint { Label = w.L, TotalCount = sub.Count, CancelCount = sub.Count(x => x.TrangThai == "huy") });
            }
            res.ForEach(r => r.Percentage = max > 0 ? ((double)r.TotalCount / max) * 100 : 0);
            return res;
        }

        private List<ChartDataPoint> GenerateMonthlyChartV2(List<DatTiec> data)
        {
            var res = new List<ChartDataPoint>();
            int max = 0;
            for (int i = 1; i <= 12; i++)
            {
                var sub = data.Where(x => x.NgayToChuc.HasValue && x.NgayToChuc.Value.Month == i).ToList();
                if (sub.Count > max) max = sub.Count;
                res.Add(new ChartDataPoint { Label = "T" + i, TotalCount = sub.Count, CancelCount = sub.Count(x => x.TrangThai == "huy") });
            }
            res.ForEach(r => r.Percentage = max > 0 ? ((double)r.TotalCount / max) * 100 : 0);
            return res;
        }

        [HttpGet]
        public IActionResult GetOrderDetail(string id)
        {
            var o = _context.DatTiecs.Include(x => x.KhachHang).Include(x => x.ChiTietThucDons).ThenInclude(m => m.MonAn).Include(x => x.TT_SuDungDichVus).ThenInclude(d => d.DichVu).FirstOrDefault(x => x.MaDatTiec == id);
            if (o == null) return NotFound();
            bool isHuy = o.TrangThai == "huy";
            return Json(new
            {
                MaDatTiec = o.MaDatTiec,
                NgayToChuc = o.NgayToChuc?.ToString("dd/MM/yyyy"),
                GioToChuc = o.GioToChuc?.ToString(@"hh\:mm"),
                SoBan = o.SoBanDat,
                TenKhach = o.KhachHang?.TenKhachHang,
                Sdt = o.KhachHang?.SdtKhachHang,
                GhiChu = isHuy ? o.ChiTiet : "Không có ghi chú",
                TongTien = FormatCurrency((o.GiaBan ?? 0) * (o.SoBanDat ?? 0)),
                TienCoc = FormatCurrency(o.TienCoc ?? 0),
                TrangThai = isHuy ? "Đã hủy" : o.TrangThai,
                MonAn = o.ChiTietThucDons.Select(m => new { TenMon = m.MonAn.TenMonAn, SoLuong = m.SoLuongMotBan, DonGia = FormatCurrency(m.MonAn.DonGia ?? 0) }).ToList(),
                DichVu = o.TT_SuDungDichVus.Select(d => new { TenDV = d.DichVu.TenDichVu, SoLuong = d.SoLuong, DonGia = FormatCurrency(d.DichVu.GiaDV) }).ToList()
            });
        }
    }
}