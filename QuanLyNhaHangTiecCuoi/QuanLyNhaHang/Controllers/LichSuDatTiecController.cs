using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.Controllers
{
    public class LichSuDatTiecController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public LichSuDatTiecController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Lấy mã khách hàng từ Session (Tên biến phải khớp 100% với DangNhapController)
            string currentMaKH = HttpContext.Session.GetString("MaKhachHang");

    // 2. Nếu Session trống (do restart dự án), lấy từ Cookie Claim "MaKH"
    if (string.IsNullOrEmpty(currentMaKH))
            {
                 currentMaKH = User.Claims.FirstOrDefault(c => c.Type == "MaKH")?.Value; 
    }

            // 3. KIỂM TRA QUAN TRỌNG: Nếu chưa đăng nhập thì chuyển về trang DANG NHAP
            // Không dùng Redirect sang Home vì sẽ gây hiểu lầm
            if (string.IsNullOrEmpty(currentMaKH))
            {
                 return RedirectToAction("Index", "DangNhap"); 
    }

            // 4. Truy vấn dữ liệu
            var listDatTiec = await _context.DatTiecs
                                    .Where(x => x.MaKhachHang == currentMaKH)
                                    .OrderByDescending(x => x.NgayDatTiec)
                                    .ToListAsync();

            return View(listDatTiec);
        }
        [HttpGet]
        public async Task<IActionResult> XacNhanHuy(string id)
        {
            if (id == null) return NotFound();

            var datTiec = await _context.DatTiecs.FindAsync(id);
            if (datTiec == null) return NotFound();

            // Nếu đơn đã hủy hoặc hoàn thành thì không cho vào trang này
            if (datTiec.TrangThai == "Huỷ đơn" || datTiec.TrangThai == "Đã thanh toán")
            {
                return RedirectToAction(nameof(Index));
            }

            return View(datTiec);
        }

        // 3. Xử lý Hủy đơn (POST) - Khi người dùng bấm nút xác nhận
        [HttpPost, ActionName("HuyDon")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyDonConfirmed(string id)
        {
            var datTiec = await _context.DatTiecs.FindAsync(id);
            if (datTiec != null)
            {
                datTiec.TrangThai = "Hủy đơn"; // Cập nhật trạng thái
                _context.DatTiecs.Update(datTiec);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ChiTiet(string id)
        {
            if (id == null) return NotFound();

            var datTiec = await _context.DatTiecs
                // Bắt buộc phải có ThenInclude(ct => ct.MonAn) để lấy Tên và Giá món
                .Include(d => d.ChiTietThucDons)
                    .ThenInclude(ct => ct.MonAn)

                // Bắt buộc phải có ThenInclude(dv => dv.DichVu) để lấy Tên và Giá dịch vụ
                .Include(d => d.TT_SuDungDichVus)
                    .ThenInclude(dv => dv.DichVu)

                .FirstOrDefaultAsync(m => m.MaDatTiec == id);

            if (datTiec == null) return NotFound();

            return View(datTiec);
        }
    }
}

