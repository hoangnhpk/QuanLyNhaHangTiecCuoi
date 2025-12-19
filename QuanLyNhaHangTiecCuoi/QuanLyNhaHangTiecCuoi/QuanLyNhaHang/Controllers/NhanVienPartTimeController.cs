using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class NhanVienPartTimeController : Controller
    {
        private readonly QuanLyNhaHangContext _context;
        public NhanVienPartTimeController(QuanLyNhaHangContext context) { _context = context; }

        public async Task<IActionResult> Index(string tuKhoa)
        {
            var query = _context.NhanVienPartTimes.AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(n => n.TenNhanVienPT.Contains(tuKhoa) ||
                                         n.MaNhanVienPT.Contains(tuKhoa) ||
                                         n.SdtNVPT.Contains(tuKhoa));
            }

            ViewData["CurrentFilter"] = tuKhoa;
            return View(await query.ToListAsync());
        }

        public IActionResult Them() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(NhanVienPartTime nv)
        {
            ModelState.Remove("TT_SuDungNhanViens");

            if (_context.NhanVienPartTimes.Any(x => x.MaNhanVienPT == nv.MaNhanVienPT))
                ModelState.AddModelError("MaNhanVienPT", "Mã nhân viên này đã tồn tại.");

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(nv.TrangThaiNV)) nv.TrangThaiNV = "Đang làm";
                    _context.Add(nv);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm NV Part-time thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            return View(nv);
        }

        public async Task<IActionResult> Sua(string id)
        {
            if (id == null) return NotFound();
            var nv = await _context.NhanVienPartTimes.FindAsync(id);
            if (nv == null) return NotFound();
            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(string id, NhanVienPartTime nv)
        {
            if (id != nv.MaNhanVienPT) return NotFound();
            ModelState.Remove("TT_SuDungNhanViens");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nv);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }
            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(string id)
        {
            var nv = await _context.NhanVienPartTimes.FindAsync(id);
            if (nv != null)
            {
                try
                {
                    _context.NhanVienPartTimes.Remove(nv);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đã xóa nhân viên Part-time";
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Không thể xóa (Nhân viên này đã có dữ liệu làm việc).";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}