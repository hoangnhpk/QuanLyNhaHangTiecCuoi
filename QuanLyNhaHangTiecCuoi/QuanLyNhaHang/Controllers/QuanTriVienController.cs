using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    public class QuanTriVienController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public QuanTriVienController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string tuKhoa)
        {
            var query = _context.QuanTriViens.AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(q => q.TenQuanTriVien.Contains(tuKhoa) ||
                                         q.SdtNV.Contains(tuKhoa) ||
                                         q.MaQuanTriVien.Contains(tuKhoa));
            }

            ViewBag.TuKhoa = tuKhoa;
            return View(await query.ToListAsync());
        }

        public IActionResult Them()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(QuanTriVien qtv)
        {
            ModelState.Remove("TaiKhoan");

            if (string.IsNullOrWhiteSpace(qtv.MaTaiKhoan))
            {
                qtv.MaTaiKhoan = null;
                ModelState.Remove("MaTaiKhoan"); 
            }
            else
            {
                var tkTonTai = await _context.TaiKhoans.AnyAsync(t => t.MaTaiKhoan == qtv.MaTaiKhoan);
                if (!tkTonTai)
                {
                    ModelState.AddModelError("MaTaiKhoan", "Mã tài khoản này không tồn tại trong hệ thống.");
                }
            }

            if (await _context.QuanTriViens.AnyAsync(x => x.MaQuanTriVien == qtv.MaQuanTriVien))
            {
                ModelState.AddModelError("MaQuanTriVien", "Mã quản trị viên này đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(qtv.TrangThai)) qtv.TrangThai = "Hoạt động";

                    _context.Add(qtv);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm quản trị viên thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", $"Lỗi hệ thống: {errorMsg}");
                }
            }

            return View(qtv);
        }

        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var qtv = await _context.QuanTriViens.FindAsync(id);
            if (qtv == null) return NotFound();

            return View(qtv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(string id, QuanTriVien qtv)
        {
            if (id != qtv.MaQuanTriVien) return NotFound();

            ModelState.Remove("TaiKhoan");
            if (string.IsNullOrWhiteSpace(qtv.MaTaiKhoan))
            {
                qtv.MaTaiKhoan = null;
                ModelState.Remove("MaTaiKhoan");
            }
            else
            {
                var tkTonTai = await _context.TaiKhoans.AnyAsync(t => t.MaTaiKhoan == qtv.MaTaiKhoan);
                if (!tkTonTai)
                {
                    ModelState.AddModelError("MaTaiKhoan", "Mã tài khoản không tồn tại.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.QuanTriViens.FindAsync(id);
                    if (existing == null) return NotFound();

                    existing.TenQuanTriVien = qtv.TenQuanTriVien;
                    existing.MaTaiKhoan = qtv.MaTaiKhoan;
                    existing.Cccd = qtv.Cccd;
                    existing.SdtNV = qtv.SdtNV;
                    existing.DiaChi = qtv.DiaChi;
                    existing.Email = qtv.Email;
                    existing.TrangThai = qtv.TrangThai;
                    existing.GhiChu = qtv.GhiChu;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", $"Lỗi cập nhật: {errorMsg}");
                }
            }
            return View(qtv);
        }
    }
}