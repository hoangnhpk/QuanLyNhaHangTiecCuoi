using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class QL_KhachHangController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public QL_KhachHangController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        // GET: QL_KhachHang
        public async Task<IActionResult> Index()
        {
            var khachHangs = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .ToListAsync();
            return View(khachHangs);
        }

        // Phương thức giả định hash mật khẩu
        private string HashPassword(string password)
        {
            // ⚠️ CẦN THAY THẾ bằng thuật toán mã hóa mật khẩu an toàn (như BCrypt, PBKDF2).
            return $"HASHED_{password}"; // 👈 Đây chỉ là giá trị giả định
        }

        // GET: QL_KhachHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: QL_KhachHang/Create
        public IActionResult Create()
        {
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan");
            ViewBag.TrangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Hoạt động", Text = "Hoạt động" },
                new SelectListItem { Value = "Khóa", Text = "Khóa" }
            };
            return View();
        }

        // POST: QL_KhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,TenKhachHang,CccdKhachHang,SdtKhachHang,DiaChiKhachHang,TrangThaiKhachHang,GhiChu,MaTaiKhoan")] KhachHang khachHang)
        {
            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan");

            // Set default values
            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
            khachHang.TrangThaiKhachHang ??= "Hoạt động";
            khachHang.GhiChu ??= string.Empty;
            khachHang.MaTaiKhoan ??= null;

            // Kiểm tra trùng mã
            if (await KhachHangExistsAsync(khachHang.MaKhachHang))
            {
                ModelState.AddModelError("MaKhachHang", $"Mã khách hàng '{khachHang.MaKhachHang}' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm Khách hàng mới thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan", khachHang.MaTaiKhoan);
            ViewBag.TrangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Hoạt động", Text = "Hoạt động" },
                new SelectListItem { Value = "Khóa", Text = "Khóa" }
            };
            return View(khachHang);
        }

        // GET: QL_KhachHang/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            // Reset password field in view
            if (khachHang.TaiKhoan != null)
            {
                khachHang.TaiKhoan.Password = null;
            }

            ViewBag.TrangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Hoạt động", Text = "Hoạt động", Selected = khachHang.TrangThaiKhachHang == "Hoạt động" },
                new SelectListItem { Value = "Khóa", Text = "Khóa", Selected = khachHang.TrangThaiKhachHang == "Khóa" }
            };

            return View(khachHang);
        }

        // POST: QL_KhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("MaKhachHang,TenKhachHang,CccdKhachHang,SdtKhachHang,DiaChiKhachHang,TrangThaiKhachHang,GhiChu,MaTaiKhoan,TaiKhoan")] KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            var originalKhachHang = await _context.KhachHangs
                .Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (originalKhachHang == null)
            {
                return NotFound();
            }

            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan");

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin khách hàng
                    originalKhachHang.TenKhachHang = khachHang.TenKhachHang ?? string.Empty;
                    originalKhachHang.CccdKhachHang = khachHang.CccdKhachHang ?? string.Empty;
                    originalKhachHang.SdtKhachHang = khachHang.SdtKhachHang ?? string.Empty;
                    originalKhachHang.DiaChiKhachHang = khachHang.DiaChiKhachHang ?? string.Empty;
                    originalKhachHang.TrangThaiKhachHang = khachHang.TrangThaiKhachHang ?? "Hoạt động";
                    originalKhachHang.GhiChu = khachHang.GhiChu ?? string.Empty;

                    // Cập nhật thông tin tài khoản nếu có
                    if (originalKhachHang.TaiKhoan != null && khachHang.TaiKhoan != null)
                    {
                        var newPasswordFromForm = khachHang.TaiKhoan.Password;

                        if (!string.IsNullOrEmpty(newPasswordFromForm))
                        {
                            originalKhachHang.TaiKhoan.Password = HashPassword(newPasswordFromForm);
                        }

                        originalKhachHang.TaiKhoan.Email = khachHang.TaiKhoan.Email ?? string.Empty;
                        originalKhachHang.TaiKhoan.UserName = khachHang.TaiKhoan.UserName ?? string.Empty;
                        originalKhachHang.TaiKhoan.TrangThai = khachHang.TaiKhoan.TrangThai ?? "Hoạt động";
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Cập nhật Khách hàng '{khachHang.MaKhachHang}' thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKhachHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.TrangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Hoạt động", Text = "Hoạt động", Selected = khachHang.TrangThaiKhachHang == "Hoạt động" },
                new SelectListItem { Value = "Khóa", Text = "Khóa", Selected = khachHang.TrangThaiKhachHang == "Khóa" }
            };

            return View(khachHang);
        }

        // GET: QL_KhachHang/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: QL_KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang != null)
            {
                try
                {
                    // Xóa Tài khoản liên quan trước
                    if (khachHang.TaiKhoan != null)
                    {
                        _context.TaiKhoans.Remove(khachHang.TaiKhoan);
                    }

                    _context.KhachHangs.Remove(khachHang);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Đã xóa khách hàng '{khachHang.TenKhachHang}' (Mã: {id}) thành công.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi xóa khách hàng '{id}'. Khách hàng có thể đang liên kết với các dữ liệu khác.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: QL_KhachHang/Lock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã khách hàng để khóa.";
                return RedirectToAction(nameof(Index));
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null)
            {
                TempData["ErrorMessage"] = "Khách hàng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem khách hàng đã bị khóa chưa
            if (khachHang.TrangThaiKhachHang == "Khóa")
            {
                TempData["ErrorMessage"] = $"Khách hàng '{khachHang.TenKhachHang}' đã bị khóa trước đó.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Khóa thông tin Khách hàng
                khachHang.TrangThaiKhachHang = "Khóa";

                // Khóa Tài khoản đăng nhập (nếu có)
                if (khachHang.TaiKhoan != null)
                {
                    khachHang.TaiKhoan.TrangThai = "Khóa";
                }

                _context.Update(khachHang);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã khóa khách hàng '{khachHang.TenKhachHang}' (Mã: {khachHang.MaKhachHang}) thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Lỗi đồng thời khi khóa tài khoản.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: QL_KhachHang/Unlock/5 (THÊM MỚI - MỞ KHÓA)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã khách hàng để mở khóa.";
                return RedirectToAction(nameof(Index));
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null)
            {
                TempData["ErrorMessage"] = "Khách hàng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem khách hàng đã hoạt động chưa
            if (khachHang.TrangThaiKhachHang == "Hoạt động")
            {
                TempData["ErrorMessage"] = $"Khách hàng '{khachHang.TenKhachHang}' đã ở trạng thái hoạt động.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Mở khóa thông tin Khách hàng
                khachHang.TrangThaiKhachHang = "Hoạt động";

                // Mở khóa Tài khoản đăng nhập (nếu có)
                if (khachHang.TaiKhoan != null)
                {
                    khachHang.TaiKhoan.TrangThai = "Hoạt động";
                }

                _context.Update(khachHang);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã mở khóa khách hàng '{khachHang.TenKhachHang}' (Mã: {khachHang.MaKhachHang}) thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Lỗi đồng thời khi mở khóa tài khoản.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }

        private async Task<bool> KhachHangExistsAsync(string id)
        {
            return await _context.KhachHangs.AnyAsync(e => e.MaKhachHang == id);
        }
    }
}