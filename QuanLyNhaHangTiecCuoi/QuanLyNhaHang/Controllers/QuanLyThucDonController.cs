using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLyNhaHang.Controllers
{
    public class QuanLyThucDonController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public QuanLyThucDonController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.MonAns.OrderBy(m => m.MaMonAn).ToListAsync();
            return View(list);
        }

        // --- XỬ LÝ THÊM (CREATE) ---
        public IActionResult Them()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(MonAn model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng mã món ăn
                if (await _context.MonAns.AnyAsync(x => x.MaMonAn == model.MaMonAn))
                {
                    ModelState.AddModelError("MaMonAn", "Mã món ăn đã tồn tại!");
                    return View(model);
                }

                _context.MonAns.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm món ăn thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // --- XỬ LÝ SỬA (EDIT) ---
        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var item = await _context.MonAns.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(MonAn model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = await _context.MonAns.FindAsync(model.MaMonAn);
                    if (item == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật TẤT CẢ các trường
                    item.TenMonAn = model.TenMonAn;
                    item.DonViTinh = model.DonViTinh;
                    item.DonGia = model.DonGia;
                    item.LoaiMonAn = model.LoaiMonAn;
                    item.TrangThaiMonAn = model.TrangThaiMonAn;
                    item.MoTaMonAn = model.MoTaMonAn;
                    item.GhiChu = model.GhiChu;

                    _context.MonAns.Update(item);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật món ăn thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật. Vui lòng thử lại.");
                    return View(model);
                }
            }

            return View(model);
        }

        // --- HÀM XÓA (DELETE) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Không tìm thấy món ăn cần xóa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var item = await _context.MonAns.FindAsync(id);
                if (item != null)
                {
                    _context.MonAns.Remove(item);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa món ăn thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy món ăn.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Không thể xóa món ăn. Có thể món ăn đang được sử dụng.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}