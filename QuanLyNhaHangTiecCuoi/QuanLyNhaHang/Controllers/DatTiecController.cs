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
            // 1. KIỂM TRA ĐĂNG NHẬP
            // Lấy Mã khách hàng từ Session (đã lưu lúc đăng nhập)
            var maKhachHang = HttpContext.Session.GetString("MaKhachHang");

            if (string.IsNullOrEmpty(maKhachHang))
            {
                // Nếu chưa đăng nhập -> Thông báo và chuyển sang trang Đăng nhập
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thực hiện đặt tiệc!";
                return RedirectToAction("Index", "DangNhap"); // Chuyển hướng sang Controller DangNhap
            }

            // 2. Xử lý đặt tiệc (Khi đã đăng nhập)
            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo Đơn Đặt Tiệc
                    var tiec = new DatTiec
                    {
                        MaDatTiec = PhatSinhMaTiec(),
                        MaKhachHang = maKhachHang, // Lấy trực tiếp từ Session, không cần tạo mới
                        TenCoDau = model.TenCoDau,
                        TenChuRe = model.TenChuRe,
                        NgayToChuc = model.NgayToChuc,
                        SoBanDat = model.SoBanDat,
                        NgayDatTiec = DateTime.Now,
                        TrangThai = "Mới đặt", // Trạng thái mặc định
                        GiaBan = 0,
                        TienCoc = 0,
                        ChiTiet = $"Dự phòng: {model.SoBanDuPhong} bàn. Ghi chú: {model.GhiChu}"
                    };

                    _context.DatTiecs.Add(tiec);
                    _context.SaveChanges(); // Lưu để có MaDatTiec

                    // 3. Xử lý Chi tiết thực đơn (Combo/Món lẻ)
                    if (!string.IsNullOrEmpty(model.LoaiThucDon))
                    {
                        var randomMonAns = _context.MonAns
                                            .OrderBy(r => Guid.NewGuid())
                                            .Take(5)
                                            .ToList();

                        foreach (var mon in randomMonAns)
                        {
                            var chiTiet = new ChiTietThucDon
                            {
                                MaChiTietThucDon = Guid.NewGuid().ToString().Substring(0, 20),
                                MaDatTiec = tiec.MaDatTiec,
                                MaMonAn = mon.MaMonAn,
                                SoLuongMotBan = 1,
                                GhiChuThem = "Thuộc menu: " + model.LoaiThucDon
                            };
                            _context.ChiTietThucDons.Add(chiTiet);
                        }
                        _context.SaveChanges();
                    }

                    // 4. Xử lý Dịch vụ
                    if (!string.IsNullOrEmpty(model.LoaiDichVu))
                    {
                        string tenDichVuCanTim = "";
                        if (model.LoaiDichVu == "Basic") tenDichVuCanTim = "Combo Trang Trí Cơ Bản";
                        else if (model.LoaiDichVu == "VIP") tenDichVuCanTim = "Combo Trang Trí VIP";

                        var dichVuDb = _context.DichVus.FirstOrDefault(d => d.TenDichVu == tenDichVuCanTim);

                        if (dichVuDb != null)
                        {
                            var suDungDV = new TT_SuDungDichVu
                            {
                                MaThongTinDV = Guid.NewGuid().ToString().Substring(0, 20),
                                MaDatTiec = tiec.MaDatTiec,
                                MaDichVu = dichVuDb.MaDichVu,
                                SoLuong = 1,
                                NgaySuDung = tiec.NgayToChuc,
                                GhiChu = "Gói: " + model.LoaiDichVu
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
                    TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
                    return RedirectToAction("Index", "Home");
                }
            }

            // Nếu Validation lỗi
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["ErrorMessage"] = "Vui lòng kiểm tra lại: " + string.Join(", ", errors);
            return RedirectToAction("Index", "Home");
        }

        // Hàm sinh mã Tiệc (Giữ nguyên)
        private string PhatSinhMaTiec()
        {
            var lastItem = _context.DatTiecs.OrderByDescending(x => x.MaDatTiec).FirstOrDefault();
            if (lastItem == null) return "DT001";
            int nextId = int.Parse(lastItem.MaDatTiec.Substring(2)) + 1;
            return "DT" + nextId.ToString("D3");
        }
    }
}