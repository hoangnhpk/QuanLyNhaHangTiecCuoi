using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cần thư viện này để dùng .Include
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.Controllers
{
    // Controller chuyên dụng cho Admin quản lý tiệc
    public class AdminDatTiecController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public AdminDatTiecController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        // GET: /AdminDatTiec
        public IActionResult Index()
        {
            // Lấy danh sách tiệc, KÈM THEO thông tin Khách hàng
            var danhSachTiec = _context.DatTiecs
                                .Include(d => d.KhachHang) // Nối bảng để lấy tên khách
                                .Include(d => d.PhieuThanhToan) // Lấy luôn phiếu thanh toán (nếu có) để xem tiền
                                .OrderByDescending(d => d.NgayDatTiec) // Tiệc mới nhất lên đầu
                                .ToList();

            return View(danhSachTiec);
        }
        // GET: /AdminDatTiec/GetDetails/DT00x
        public IActionResult GetDetails(string id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin tiệc + Khách hàng + Chi tiết món ăn + Dịch vụ đã chọn
            var tiec = _context.DatTiecs
                .Include(t => t.KhachHang)
                .Include(t => t.ChiTietThucDons).ThenInclude(ct => ct.MonAn) // Lấy tên món
                .Include(t => t.TT_SuDungDichVus).ThenInclude(dv => dv.DichVu) // Lấy tên dịch vụ
                .FirstOrDefault(m => m.MaDatTiec == id);

            if (tiec == null) return NotFound();

            // Trả về một PartialView (Giao diện con) để nhét vào Modal
            return PartialView("_DetailsPartial", tiec);
        }
        // POST: /AdminDatTiec/UpdateStatus
        [HttpPost]
        public IActionResult UpdateStatus(string id, string status)
        {
            var tiec = _context.DatTiecs.Find(id);
            if (tiec == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn tiệc!" });
            }

            // Cập nhật trạng thái mới
            tiec.TrangThai = status; // status sẽ là "Đã duyệt", "Hủy", "Đã thanh toán"...

            // Nếu duyệt đơn, có thể cập nhật thêm logic khác (ví dụ: gửi mail thông báo)
            // ... (để sau)

            _context.SaveChanges();

            return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
        }
        // GET: /AdminDatTiec/Search?keyword=...&status=...
        public IActionResult Search(string keyword, string status)
        {
            try
            {
                var query = _context.DatTiecs
                    .Include(d => d.KhachHang)
                    .Include(d => d.PhieuThanhToan)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    string k = keyword.ToLower().Trim();
                    query = query.Where(d =>
                        d.MaDatTiec.Contains(k) || // Tìm theo mã đơn
                        (d.KhachHang != null && d.KhachHang.TenKhachHang.ToLower().Contains(k)) ||
                        (d.KhachHang != null && d.KhachHang.SdtKhachHang.Contains(k)) ||
                        (d.TenCoDau != null && d.TenCoDau.ToLower().Contains(k)) ||
                        (d.TenChuRe != null && d.TenChuRe.ToLower().Contains(k))
                    );
                }

                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    // Giải mã URL (VD: %20 -> dấu cách)
                    string decodedStatus = System.Net.WebUtility.UrlDecode(status);
                    query = query.Where(d => d.TrangThai == decodedStatus);
                }

                var result = query.OrderByDescending(d => d.NgayDatTiec).ToList();
                return PartialView("_OrderTable", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi Server: " + ex.Message);
            }
        }
    }
}