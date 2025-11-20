using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models; // Gọi các Model
using System.Diagnostics;

namespace QuanLyNhaHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        // Inject Context vào để dùng
        public HomeController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TenKH = HttpContext.Session.GetString("TenKhachHang");
            return View();
        }

        public IActionResult LienHe()
        {
            return View();
        }

        public IActionResult ThucDon()
        {
            // Sau này sẽ lấy list món ăn từ DB truyền sang View ở đây
            return View();
        }

        // --- HÀM TẠO DỮ LIỆU MẪU (CHẠY 1 LẦN LÀ ĐỦ) ---
        public IActionResult TaoDuLieuMau()
        {
            // 1. Kiểm tra nếu có dữ liệu Món Ăn rồi thì thôi, không tạo nữa
            if (_context.MonAns.Any())
            {
                return Content("Dữ liệu đã có sẵn rồi! Không cần tạo thêm.");
            }

            // 2. Tạo Danh sách Món Ăn (Khớp với file HTML Menu của bạn)
            var listMonAn = new List<MonAn>
            {
                new MonAn { MaMonAn = "MA001", TenMonAn = "Súp Tôm Hùm", DonGia = 145000, LoaiMonAn = "Khai vị", DonViTinh = "Tô", MoTaMonAn = "Tôm hùm, kem tươi, rượu vang", TrangThaiMonAn = "Còn phục vụ" },
                new MonAn { MaMonAn = "MA002", TenMonAn = "Bánh Cua", DonGia = 195000, LoaiMonAn = "Khai vị", DonViTinh = "Cái", MoTaMonAn = "Thịt cua tươi, sốt tartar", TrangThaiMonAn = "Còn phục vụ" },
                new MonAn { MaMonAn = "MA003", TenMonAn = "Phô Mai Que", DonGia = 125000, LoaiMonAn = "Khai vị", DonViTinh = "Đĩa", MoTaMonAn = "Phô mai mozzarella chiên giòn", TrangThaiMonAn = "Còn phục vụ" },

                new MonAn { MaMonAn = "MA004", TenMonAn = "Salad Caesar", DonGia = 219000, LoaiMonAn = "Salad", DonViTinh = "Đĩa", MoTaMonAn = "Xà lách romaine, phô mai parmesan", TrangThaiMonAn = "Còn phục vụ" },
                new MonAn { MaMonAn = "MA005", TenMonAn = "Salad Hy Lạp", DonGia = 245000, LoaiMonAn = "Salad", DonViTinh = "Đĩa", MoTaMonAn = "Cà chua, ô liu, phô mai feta", TrangThaiMonAn = "Còn phục vụ" },

                new MonAn { MaMonAn = "MA006", TenMonAn = "Gà Nướng Tuscan", DonGia = 245000, LoaiMonAn = "Món chính", DonViTinh = "Phần", MoTaMonAn = "Gà nướng phô mai provolone", TrangThaiMonAn = "Còn phục vụ" },
                new MonAn { MaMonAn = "MA007", TenMonAn = "Bò Mỹ Sốt Truffle", DonGia = 350000, LoaiMonAn = "Món chính", DonViTinh = "Phần", MoTaMonAn = "Thịt bò mềm sốt nấm", TrangThaiMonAn = "Còn phục vụ" },

                new MonAn { MaMonAn = "MA008", TenMonAn = "Panna Cotta Dâu", DonGia = 75000, LoaiMonAn = "Tráng miệng", DonViTinh = "Ly", MoTaMonAn = "Tráng miệng kiểu Ý", TrangThaiMonAn = "Còn phục vụ" }
            };
            _context.MonAns.AddRange(listMonAn);

            // 3. Tạo Danh sách Dịch Vụ
            var listDichVu = new List<DichVu>
            {
                new DichVu { MaDichVu = "DV001", TenDichVu = "Combo Trang Trí Cơ Bản", GiaDV = 15000000, MoTaDV = "Backdrop, bàn ghế tiêu chuẩn", TrangThaiDV = "Hoạt động" },
                new DichVu { MaDichVu = "DV002", TenDichVu = "Combo Trang Trí VIP", GiaDV = 50000000, MoTaDV = "Hoa tươi, đèn LED, sân khấu 3D", TrangThaiDV = "Hoạt động" },
                new DichVu { MaDichVu = "DV003", TenDichVu = "Ban Nhạc Acoustic", GiaDV = 10000000, MoTaDV = "Ban nhạc biểu diễn 2 tiếng", TrangThaiDV = "Hoạt động" },
                new DichVu { MaDichVu = "DV004", TenDichVu = "MC Chuyên Nghiệp", GiaDV = 5000000, MoTaDV = "Dẫn chương trình tiệc cưới", TrangThaiDV = "Hoạt động" }
            };
            _context.DichVus.AddRange(listDichVu);

            // 4. Tạo Khách Hàng Mẫu
            var khach = new KhachHang
            {
                MaKhachHang = "KH001",
                TenKhachHang = "Nguyễn Văn Demo",
                SdtKhachHang = "0909123456",
                //EmailKhachHang = "demo@gmail.com",
                //TaiKhoanKhachHang = "khach01",
                //MatKhauKhachHang = "123456",
                TrangThaiKhachHang = "Hoạt động"
            };
            _context.KhachHangs.Add(khach);
            _context.SaveChanges(); // Lưu trước để lấy mã KH001 dùng cho bên dưới

            // 5. Tạo Tiệc Mẫu (Đã đặt)
            var tiec = new DatTiec
            {
                MaDatTiec = "DT001",
                MaKhachHang = "KH001",
                TenChuRe = "Văn Demo",
                TenCoDau = "Thị Mẫu",
                NgayDatTiec = DateTime.Now.AddDays(-5),
                NgayToChuc = DateTime.Now.AddDays(20),
                SoBanDat = 30,
                TrangThai = "Đã cọc",
                GiaBan = 3500000, // Giá trung bình 1 bàn
                TienCoc = 10000000
            };
            _context.DatTiecs.Add(tiec);
            _context.SaveChanges();

            return Content("Đã khởi tạo dữ liệu mẫu thành công! Hãy vào SQL kiểm tra.");
        }
    }
}