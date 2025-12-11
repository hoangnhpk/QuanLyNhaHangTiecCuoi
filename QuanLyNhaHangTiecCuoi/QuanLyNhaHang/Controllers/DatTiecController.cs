using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Models.ViewModels;
using Newtonsoft.Json;

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
                return RedirectToAction("Index", "DangNhap"); // Chuyển hướng sang Controller DangNhap
            }

            // 2. Xử lý đặt tiệc (Khi đã đăng nhập)
            if (ModelState.IsValid)
            {
                if (model.NgayToChuc.HasValue)
                {
                    // Kiểm tra trong DB xem có đơn nào trùng ngày này không
                    // Lưu ý: Chỉ tính những đơn chưa bị "Hủy"
                    bool daCoTiec = _context.DatTiecs.Any(d =>
                        d.NgayToChuc.HasValue &&
                        d.NgayToChuc.Value.Date == model.NgayToChuc.Value.Date &&
                        d.TrangThai != "Hủy" // Nếu đơn cũ đã Hủy thì vẫn cho đặt lại
                    );

                    if (daCoTiec)
                    {
                        // Thông báo lỗi và trả về trang chủ
                        TempData["ErrorMessage"] = $"Rất tiếc, ngày {model.NgayToChuc.Value:dd/MM/yyyy} nhà hàng đã có tiệc. Vui lòng chọn ngày khác!";
                        return Redirect(Url.Action("Index", "Home") + "#book-a-table");
                    }
                }
                try
                {
                    // --- BƯỚC 1: TÍNH TOÁN GIÁ TIỀN (SERVER SIDE) ---
                    decimal giaThucDonMoiBan = 0;
                    decimal giaDichVuTotal = 0;

                    // A. Tính tiền Thực đơn
                    if (model.LoaiThucDon == "TuChon" && !string.IsNullOrEmpty(model.ThucDonTuChonJson))
                    {
                        var danhSachMon = JsonConvert.DeserializeObject<List<MonTuChonItem>>(model.ThucDonTuChonJson);
                        if (danhSachMon != null)
                        {
                            foreach (var item in danhSachMon)
                            {
                                if (string.IsNullOrEmpty(item.MaMonAn)) continue;

                                // QUAN TRỌNG: Lấy giá gốc từ Database để tính, không lấy giá từ JSON gửi lên
                                var monDb = _context.MonAns.Find(item.MaMonAn);
                                if (monDb != null && monDb.DonGia.HasValue)
                                {
                                    giaThucDonMoiBan += (monDb.DonGia.Value * item.SoLuong);
                                }
                            }
                        }
                    }
                    else if (model.LoaiThucDon == "Vàng")
                    {
                        giaThucDonMoiBan = 1000000; // Giá cứng hoặc lấy từ DB bảng Combo
                    }
                    else if (model.LoaiThucDon == "Bạch Kim")
                    {
                        giaThucDonMoiBan = 2000000;
                    }

                    // B. Tính tiền Dịch vụ
                    if (model.LoaiDichVu == "Basic") giaDichVuTotal = 15000000;
                    else if (model.LoaiDichVu == "VIP") giaDichVuTotal = 50000000;

                    // C. Tổng kết con số
                    int soBan = model.SoBanDat ?? 0;
                    decimal tongTienHopDong = (giaThucDonMoiBan * soBan) + giaDichVuTotal;
                    decimal tienCocPhaiDong = tongTienHopDong * 0.3m; // 30%

                    // --- BƯỚC 2: TẠO ĐƠN HÀNG VÀ LƯU DATABASE ---
                    var tiec = new DatTiec
                    {
                        MaDatTiec = PhatSinhMaTiec(),
                        MaKhachHang = maKhachHang,
                        TenCoDau = model.TenCoDau,
                        TenChuRe = model.TenChuRe,
                        NgayToChuc = model.NgayToChuc,
                        GioToChuc = model.GioToChuc, // Lưu giờ
                        SoBanDat = soBan,
                        NgayDatTiec = DateTime.Now,
                        TrangThai = "Mới đặt",

                        // Lưu giá tiền đã tính toán
                        GiaBan = tongTienHopDong, // Lưu tổng giá trị hợp đồng
                        TienCoc = tienCocPhaiDong,

                        ChiTiet = $"Dự phòng: {model.SoBanDuPhong} bàn. Ghi chú: {model.GhiChu}"
                    };

                    _context.DatTiecs.Add(tiec);
                    _context.SaveChanges(); // Lưu xong để có MaDatTiec dùng bên dưới

                    // --- BƯỚC 3: LƯU CHI TIẾT THỰC ĐƠN (GIỮ NGUYÊN CODE CŨ) ---
                    if (!string.IsNullOrEmpty(model.LoaiThucDon))
                    {
                        if (model.LoaiThucDon == "TuChon" && !string.IsNullOrEmpty(model.ThucDonTuChonJson))
                        {
                            var danhSachMon = JsonConvert.DeserializeObject<List<MonTuChonItem>>(model.ThucDonTuChonJson);
                            if (danhSachMon != null)
                            {
                                foreach (var item in danhSachMon)
                                {
                                    if (string.IsNullOrEmpty(item.MaMonAn)) continue;
                                    var chiTiet = new ChiTietThucDon
                                    {
                                        MaChiTietThucDon = Guid.NewGuid().ToString().Substring(0, 20),
                                        MaDatTiec = tiec.MaDatTiec,
                                        MaMonAn = item.MaMonAn,
                                        SoLuongMotBan = item.SoLuong,
                                        GhiChuThem = "Món tự chọn"
                                    };
                                    _context.ChiTietThucDons.Add(chiTiet);
                                }
                            }
                        }
                        else
                        {
                            // Logic Random món cho Combo (Giữ nguyên như cũ)
                            var randomMonAns = _context.MonAns
                                                .Where(m => m.LoaiMonAn != "Nước Uống")
                                                .OrderBy(r => Guid.NewGuid())
                                                .Take(5).ToList();
                            foreach (var mon in randomMonAns)
                            {
                                _context.ChiTietThucDons.Add(new ChiTietThucDon
                                {
                                    MaChiTietThucDon = Guid.NewGuid().ToString().Substring(0, 20),
                                    MaDatTiec = tiec.MaDatTiec,
                                    MaMonAn = mon.MaMonAn,
                                    SoLuongMotBan = 1,
                                    GhiChuThem = "Menu: " + model.LoaiThucDon
                                });
                            }
                        }
                        _context.SaveChanges();
                    }

                    // --- BƯỚC 4: LƯU DỊCH VỤ (GIỮ NGUYÊN) ---
                    if (!string.IsNullOrEmpty(model.LoaiDichVu))
                    {
                        string tenDichVuCanTim = (model.LoaiDichVu == "Basic") ? "Combo Trang Trí Cơ Bản" : "Combo Trang Trí VIP";
                        var dichVuDb = _context.DichVus.FirstOrDefault(d => d.TenDichVu == tenDichVuCanTim);

                        if (dichVuDb != null)
                        {
                            _context.TT_SuDungDichVus.Add(new TT_SuDungDichVu
                            {
                                MaThongTinDV = Guid.NewGuid().ToString().Substring(0, 20),
                                MaDatTiec = tiec.MaDatTiec,
                                MaDichVu = dichVuDb.MaDichVu,
                                SoLuong = 1,
                                NgaySuDung = tiec.NgayToChuc,
                                GhiChu = "Gói: " + model.LoaiDichVu
                            });
                            _context.SaveChanges();
                        }
                    }

                    TempData["SuccessMessage"] = $"Đặt tiệc thành công! Tổng tiền: {tongTienHopDong:N0} VNĐ. Vui lòng thanh toán cọc: {tienCocPhaiDong:N0} VNĐ";
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
        // GET: /DatTiec/GetThucDonTuChon
        [HttpGet]
        public IActionResult GetThucDonTuChon()
        {
            // 1. Lấy tất cả món ăn đang phục vụ
            var danhSachMon = _context.MonAns
                .Where(m => m.TrangThaiMonAn != "Ngừng phục vụ")
                .OrderBy(m => m.LoaiMonAn) // Sắp xếp để Group đẹp hơn
                .ToList();

            // 2. Nhóm món ăn theo Loại
            // Kết quả trả về là IEnumerable<IGrouping<string, MonAn>> -> Khớp với @model trong View
            var danhSachNhom = danhSachMon.GroupBy(m => m.LoaiMonAn).ToList();

            return PartialView("_ThucDonTuChon", danhSachNhom);
        }
        // Class nhỏ giúp giải mã JSON
        public class MonTuChonItem
        {
            public string MaMonAn { get; set; }
            public string TenMonAn { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
        }
    }
}