using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    [Authorize(Roles = "Admin")]
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
            //  xóa lỗi validate của nó đi (nếu không ModelState sẽ luôn False)
            ModelState.Remove("TaiKhoan.Email");
            ModelState.Remove("TaiKhoan.VaiTro");
            ModelState.Remove("TaiKhoan.TrangThai");

            // Xóa lỗi MaTaiKhoan của bảng QuanTriVien (vì lúc đầu nó null, lưu xong TaiKhoan mới có)
            ModelState.Remove("MaTaiKhoan");

            if (qtv.TaiKhoan == null)
            {
                qtv.TaiKhoan = new TaiKhoan();
            }

            // Tự động gán dữ liệu từ QTV sang TaiKhoan
            qtv.TaiKhoan.Email = qtv.Email;
            qtv.TaiKhoan.VaiTro = "Quản Lý";
            qtv.TaiKhoan.TrangThai = qtv.TrangThai ?? "Hoạt động";

            if (!string.IsNullOrEmpty(qtv.TaiKhoan.MaTaiKhoan))
            {
                if (await _context.TaiKhoans.AnyAsync(t => t.MaTaiKhoan == qtv.TaiKhoan.MaTaiKhoan))
                {
                    ModelState.AddModelError("TaiKhoan.MaTaiKhoan", "Mã tài khoản đã tồn tại.");
                }
            }

            if (!string.IsNullOrEmpty(qtv.TaiKhoan.UserName))
            {
                if (await _context.TaiKhoans.AnyAsync(t => t.UserName == qtv.TaiKhoan.UserName))
                {
                    ModelState.AddModelError("TaiKhoan.UserName", "Tên đăng nhập đã tồn tại.");
                }
            }

            if (await _context.QuanTriViens.AnyAsync(x => x.MaQuanTriVien == qtv.MaQuanTriVien))
            {
                ModelState.AddModelError("MaQuanTriVien", "Mã quản trị viên này đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Lưu Tài khoản
                        _context.TaiKhoans.Add(qtv.TaiKhoan);
                        await _context.SaveChangesAsync();

                        // Lấy mã TK vừa lưu gán cho QTV
                        qtv.MaTaiKhoan = qtv.TaiKhoan.MaTaiKhoan;

                        // Lưu QTV
                        if (string.IsNullOrEmpty(qtv.TrangThai)) qtv.TrangThai = "Hoạt động";
                        _context.QuanTriViens.Add(qtv);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        TempData["SuccessMessage"] = "Thêm mới thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        ModelState.AddModelError("", $"Lỗi hệ thống: {errorMsg}");
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi Validation: " + error.ErrorMessage);
                }
            }

            return View(qtv);
        }

        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var qtv = await _context.QuanTriViens
                .Include(t => t.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaQuanTriVien == id);

            if (qtv == null) return NotFound();

            return View(qtv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(string id, QuanTriVien qtv)
        {
            if (id != qtv.MaQuanTriVien) return NotFound();

          
            ModelState.Remove("TaiKhoan.MaTaiKhoan");
            ModelState.Remove("TaiKhoan.Email");
            ModelState.Remove("TaiKhoan.VaiTro");
            ModelState.Remove("TaiKhoan.TrangThai");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.QuanTriViens
                        .Include(x => x.TaiKhoan)
                        .FirstOrDefaultAsync(x => x.MaQuanTriVien == id);

                    if (existing == null) return NotFound();

                    //  Cập nhật thông tin Quản trị viên
                    existing.TenQuanTriVien = qtv.TenQuanTriVien;
                    existing.Cccd = qtv.Cccd;
                    existing.SdtNV = qtv.SdtNV;
                    existing.DiaChi = qtv.DiaChi;
                    existing.Email = qtv.Email;
                    existing.TrangThai = qtv.TrangThai;
                    existing.GhiChu = qtv.GhiChu;

                    //  Cập nhật thông tin Tài khoản (Nếu nhân viên này CÓ tài khoản)
                    if (existing.TaiKhoan != null && qtv.TaiKhoan != null)
                    {
                        // Chỉ update UserName nếu có thay đổi và không bị trùng
                        if (existing.TaiKhoan.UserName != qtv.TaiKhoan.UserName)
                        {
                            bool trungUserName = await _context.TaiKhoans.AnyAsync(t => t.UserName == qtv.TaiKhoan.UserName);
                            if (trungUserName)
                            {
                                ModelState.AddModelError("TaiKhoan.UserName", "Tên đăng nhập đã được người khác sử dụng.");
                                return View(qtv);
                            }
                            existing.TaiKhoan.UserName = qtv.TaiKhoan.UserName;
                        }

                        // Luôn cập nhật mật khẩu mới
                        existing.TaiKhoan.Password = qtv.TaiKhoan.Password;

                        // Đồng bộ Email và Trạng thái từ QTV sang Tài khoản
                        existing.TaiKhoan.Email = qtv.Email;
                        existing.TaiKhoan.TrangThai = qtv.TrangThai;
                    }

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