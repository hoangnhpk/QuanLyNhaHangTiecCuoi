using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Models.ViewModels;

namespace QuanLyNhaHang.Controllers
{
    public class DatTiecController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public DatTiecController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult DatTiec(DatTiecVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Xử lý Khách Hàng
                    var khachHang = _context.KhachHangs.FirstOrDefault(k => k.SdtKhachHang == model.SDT);
                    if (khachHang == null)
                    {
                        khachHang = new KhachHang
                        {
                            MaKhachHang = PhatSinhMaKH(),
                            TenKhachHang = model.HoTen,
                            SdtKhachHang = model.SDT,
                            //EmailKhachHang = model.Email,
                            TrangThaiKhachHang = "Tiềm năng"
                        };
                        _context.KhachHangs.Add(khachHang);
                        _context.SaveChanges();
                    }

                    // 2. Tạo Đơn Đặt Tiệc
                    var tiec = new DatTiec
                    {
                        MaDatTiec = PhatSinhMaTiec(),
                        MaKhachHang = khachHang.MaKhachHang,
                        TenCoDau = model.TenCoDau,
                        TenChuRe = model.TenChuRe,
                        NgayToChuc = model.NgayToChuc,
                        SoBanDat = model.SoBanDat,
                        NgayDatTiec = DateTime.Now,
                        TrangThai = "Mới đặt",
                        GiaBan = 0, // Tạm thời để 0 hoặc giá mặc định để tránh lỗi database
                        TienCoc = 0,
                        ChiTiet = $"Dự phòng: {model.SoBanDuPhong} bàn. Ghi chú: {model.GhiChu}"
                    };

                    _context.DatTiecs.Add(tiec);
                    _context.SaveChanges();


                    // --- 3. XỬ LÝ THÊM CHI TIẾT THỰC ĐƠN (MỚI) ---
                    if (!string.IsNullOrEmpty(model.LoaiThucDon))
                    {
                        // Lấy ngẫu nhiên 5 món ăn từ Database
                        // Guid.NewGuid() là mẹo để sort ngẫu nhiên trong SQL
                        var randomMonAns = _context.MonAns
                                            .OrderBy(r => Guid.NewGuid())
                                            .Take(5)
                                            .ToList();

                        foreach (var mon in randomMonAns)
                        {
                            var chiTiet = new ChiTietThucDon
                            {
                                MaChiTietThucDon = Guid.NewGuid().ToString().Substring(0, 20), // Tạo ID ngẫu nhiên ngắn
                                MaDatTiec = tiec.MaDatTiec,
                                MaMonAn = mon.MaMonAn,
                                SoLuongMotBan = 1, // Mặc định 1 phần/bàn
                                GhiChuThem = "Món theo set " + model.LoaiThucDon
                            };
                            _context.ChiTietThucDons.Add(chiTiet);
                        }
                        _context.SaveChanges();
                    }

                    if (!string.IsNullOrEmpty(model.LoaiDichVu))
                    {
                        // Xác định tên dịch vụ cần tìm dựa trên lựa chọn của khách
                        // (Dữ liệu này phải khớp với dữ liệu mẫu bạn đã tạo trong HomeController)
                        string tenDichVuCanTim = "";
                        if (model.LoaiDichVu == "Basic")
                        {
                            tenDichVuCanTim = "Combo Trang Trí Cơ Bản";
                        }
                        else if (model.LoaiDichVu == "VIP")
                        {
                            tenDichVuCanTim = "Combo Trang Trí VIP";
                        }

                        // Tìm dịch vụ trong Database
                        var dichVuDb = _context.DichVus.FirstOrDefault(d => d.TenDichVu == tenDichVuCanTim);

                        if (dichVuDb != null)
                        {
                            // Tạo thông tin sử dụng dịch vụ
                            var suDungDV = new TT_SuDungDichVu
                            {
                                MaThongTinDV = Guid.NewGuid().ToString().Substring(0, 20), // Tạo ID ngẫu nhiên
                                MaDatTiec = tiec.MaDatTiec,
                                MaDichVu = dichVuDb.MaDichVu,
                                SoLuong = 1, // Combo thì thường số lượng là 1
                                NgaySuDung = tiec.NgayToChuc,
                                GhiChu = "Khách chọn gói " + model.LoaiDichVu
                            };

                            _context.TT_SuDungDichVus.Add(suDungDV);
                            _context.SaveChanges();
                        }
                    }

                    TempData["SuccessMessage"] = "Đặt tiệc thành công! Mã đơn: " + tiec.MaDatTiec;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Bắt lỗi nếu lưu SQL bị fail (ví dụ thiếu trường bắt buộc)
                    TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
                    return RedirectToAction("Index", "Home");
                }
            }

            // 3. Nếu dữ liệu nhập vào bị sai (Validation False)
            // Lấy danh sách lỗi để hiển thị ra (Debug)
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            string loiNhanVien = string.Join(", ", errors);

            TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin: " + loiNhanVien;

            return RedirectToAction("Index", "Home");
        }

        // --- HÀM SINH MÃ TỰ ĐỘNG ---
        private string PhatSinhMaKH()
        {
            var lastItem = _context.KhachHangs.OrderByDescending(x => x.MaKhachHang).FirstOrDefault();
            if (lastItem == null) return "KH001";
            // Lấy số cuối (VD: KH005 -> 5)
            int nextId = int.Parse(lastItem.MaKhachHang.Substring(2)) + 1;
            return "KH" + nextId.ToString("D3");
        }

        private string PhatSinhMaTiec()
        {
            var lastItem = _context.DatTiecs.OrderByDescending(x => x.MaDatTiec).FirstOrDefault();
            if (lastItem == null) return "DT001";
            int nextId = int.Parse(lastItem.MaDatTiec.Substring(2)) + 1;
            return "DT" + nextId.ToString("D3");
        }
    }
}