using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.Controllers
{
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
        private string HashPassword(string password)
        {
            // ⚠️ CẦN THAY THẾ bằng thuật toán mã hóa mật khẩu an toàn (như BCrypt, PBKDF2).
            // Ví dụ: return BCrypt.Net.BCrypt.HashPassword(password);
            return $"HASHED_{password}"; // 👈 Đây chỉ là giá trị giả định
        }

        // ... (Các Action Index, Details, Delete, Lock, KhachHangExists giữ nguyên) ...

        // GET: QL_KhachHang/Edit/5 (Giữ nguyên)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cần Include TaiKhoan để hiển thị thông tin tài khoản trong View
            var khachHang = await _context.KhachHangs
                                         .Include(kh => kh.TaiKhoan)
                                         .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            // 💡 Quan trọng: Đảm bảo trường Mật Khẩu (Password) trong View trống
            // Nếu bạn không gán TaiKhoan = null, nó sẽ tự động load Mật khẩu đã Hash ra View
            if (khachHang.TaiKhoan != null)
            {
                khachHang.TaiKhoan.Password = null;
            }

            return View(khachHang);
        }


        // POST: QL_KhachHang/Edit/5
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
                    originalKhachHang.TenKhachHang = khachHang.TenKhachHang ?? string.Empty;
                    originalKhachHang.CccdKhachHang = khachHang.CccdKhachHang ?? string.Empty;
                    originalKhachHang.SdtKhachHang = khachHang.SdtKhachHang ?? string.Empty;
                    originalKhachHang.DiaChiKhachHang = khachHang.DiaChiKhachHang ?? string.Empty;
                    originalKhachHang.TrangThaiKhachHang = khachHang.TrangThaiKhachHang ?? "Active";
                    originalKhachHang.GhiChu = khachHang.GhiChu ?? string.Empty;

                    if (originalKhachHang.TaiKhoan != null && khachHang.TaiKhoan != null)
                    {
                        var newPasswordFromForm = khachHang.TaiKhoan.Password;

                        if (!string.IsNullOrEmpty(newPasswordFromForm))
                        {
                            originalKhachHang.TaiKhoan.Password = HashPassword(newPasswordFromForm);
                        }

                        originalKhachHang.TaiKhoan.Email = khachHang.TaiKhoan.Email ?? string.Empty;
                        originalKhachHang.TaiKhoan.UserName = khachHang.TaiKhoan.UserName ?? string.Empty;
                        originalKhachHang.TaiKhoan.TrangThai = khachHang.TaiKhoan.TrangThai ?? "Active";
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

            return View(khachHang);
        }
        // ... (Các Action Create, Delete, Lock, KhachHangExists giữ nguyên) ...
        // GET: QL_KhachHang/Create
        public IActionResult Create()
        {
            // 💡 Cung cấp danh sách MaTaiKhoan để chọn (nếu cần)
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan");
            return View();
        }

        // POST: QL_KhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // 🔒 Thêm Bind attribute cho các trường thông thường
        public async Task<IActionResult> Create([Bind("MaKhachHang,TenKhachHang,CccdKhachHang,SdtKhachHang,DiaChiKhachHang,TrangThaiKhachHang,GhiChu,MaTaiKhoan")] KhachHang khachHang)
        {
            // Loại bỏ validation cho DatTiecs và TaiKhoan
            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan");

            // Xử lý NULL values trước khi lưu
            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
            khachHang.TrangThaiKhachHang ??= "Active";
            khachHang.GhiChu ??= string.Empty;
            khachHang.MaTaiKhoan ??= null;

            // Vùng 1: KIỂM TRA TRÙNG MÃ KHI THÊM MỚI
            if (await KhachHangExistsAsync(khachHang.MaKhachHang))
            {
                ModelState.AddModelError("MaKhachHang", $"Mã khách hàng '{khachHang.MaKhachHang}' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                // ... (Logic lưu Create) ...
                _context.Add(khachHang);
                await _context.SaveChangesAsync();

                // 🔔 THÊM THÔNG BÁO THÀNH CÔNG
                TempData["SuccessMessage"] = "Thêm Khách hàng mới thành công!";

                return RedirectToAction(nameof(Index));
            }

            // 💡 Cung cấp lại danh sách MaTaiKhoan nếu ModelState không hợp lệ
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan", khachHang.MaTaiKhoan);
            return View(khachHang);
        }

        // ... (Các action khác giữ nguyên) ...

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