using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.Controllers
{
    public class NhanVienFullTimeController : Controller
    {
        private readonly QuanLyNhaHangContext _context;
        public NhanVienFullTimeController(QuanLyNhaHangContext context) { _context = context; }

        public async Task<IActionResult> Index(string tuKhoa)
        {
            var query = _context.NhanViens.Include(n => n.BoPhan).AsQueryable();

            // Xử lý tìm kiếm
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(n => n.TenNhanVien.Contains(tuKhoa) ||
                                         n.MaNhanVien.Contains(tuKhoa) ||
                                         n.SdtNV.Contains(tuKhoa));
            }

            // Lưu từ khóa để hiển thị lại trên thanh tìm kiếm
            ViewData["CurrentFilter"] = tuKhoa;

            return View(await query.ToListAsync());
        }

        public IActionResult Them() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(NhanVien nv)
        {
            // Bỏ qua check object quan hệ, nhưng VẪN check MaBoPhan (string)
            ModelState.Remove("BoPhan");
            ModelState.Remove("TT_SuDungNhanViens");

            if (_context.NhanViens.Any(x => x.MaNhanVien == nv.MaNhanVien))
                ModelState.AddModelError("MaNhanVien", "Mã nhân viên này đã tồn tại.");

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(nv.TrangThaiNV)) nv.TrangThaiNV = "Đang làm";

                    _context.Add(nv);
                    await _context.SaveChangesAsync();

                   
                    TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Lỗi hệ thống: " + msg);
                }
            }
            return View(nv);
        }

        public async Task<IActionResult> Sua(string id)
        {
            if (id == null) return NotFound();
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();
            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(string id, NhanVien nv)
        {
            if (id != nv.MaNhanVien) return NotFound();

            ModelState.Remove("BoPhan");
            ModelState.Remove("TT_SuDungNhanViens");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nv);
                    await _context.SaveChangesAsync();

                   
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Lỗi cập nhật: " + msg);
                }
            }
            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(string id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv != null)
            {
                try
                {
                    _context.NhanViens.Remove(nv);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Đã xóa nhân viên!";
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Không thể xóa nhân viên này (Dữ liệu đang được sử dụng).";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}