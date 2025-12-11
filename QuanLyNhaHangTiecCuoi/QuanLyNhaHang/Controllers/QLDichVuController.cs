using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    public class QLDichVuController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public QLDichVuController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        // GET: DichVu
        public async Task<IActionResult> Index()
        {
            ViewBag.ShowToast = TempData["ShowToast"] ?? false;
            ViewBag.ToastMessage = TempData["ToastMessage"] ?? "";
            ViewBag.ToastType = TempData["ToastType"] ?? "success";

            return View(await _context.DichVus.ToListAsync());
        }

        // GET: DichVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create - SỬA THEO CÁCH CỦA QuanLyThucDonController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DichVu dichVu, IFormFile HinhAnhDichVu)
        {
            // BỎ VALIDATION CÁC TRƯỜNG KHÔNG CẦN THIẾT - GIỐNG QuanLyThucDonController
            ModelState.Remove("TT_SuDungDichVus");
            ModelState.Remove("HinhAnhDichVu");
            ModelState.Remove("HinhAnhDichVu"); // Thêm dòng này để chắc chắn

            // Kiểm tra có chọn ảnh không - GIỐNG CÁCH KIỂM TRA TRONG QuanLyThucDonController
            if (HinhAnhDichVu == null || HinhAnhDichVu.Length == 0)
            {
                ModelState.AddModelError("HinhAnhDichVu", "Vui lòng chọn hình ảnh dịch vụ!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // XỬ LÝ UPLOAD ẢNH - GIỐNG HỆT QuanLyThucDonController
                    if (HinhAnhDichVu != null && HinhAnhDichVu.Length > 0)
                    {
                        // Tạo tên file - GIỐNG CÁCH ĐẶT TÊN TRONG QuanLyThucDonController
                        var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + HinhAnhDichVu.FileName;

                        // Đường dẫn: wwwroot/assets/img/menu - DÙNG CÙNG THƯ MỤC VỚI THỰC ĐƠN
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "menu");

                        // Tạo thư mục nếu chưa có
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        var filePath = Path.Combine(uploadDir, fileName);

                        // Lưu file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnhDichVu.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn vào DB - GIỐNG CÁCH LƯU TRONG QuanLyThucDonController
                        dichVu.HinhAnhDichVu = "/assets/img/menu/" + fileName;
                    }

                    _context.Add(dichVu);
                    await _context.SaveChangesAsync();

                    TempData["ToastMessage"] = "Thêm dịch vụ thành công!";
                    TempData["ToastType"] = "success";
                    TempData["ToastTitle"] = "Thành công";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // XỬ LÝ LỖI AN TOÀN HƠN
                    TempData["ToastMessage"] = "Lỗi khi thêm dịch vụ. Vui lòng thử lại!";
                    TempData["ToastType"] = "error";
                    TempData["ToastTitle"] = "Lỗi";

                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            else
            {
                TempData["ToastMessage"] = "Vui lòng kiểm tra lại thông tin nhập!";
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi dữ liệu";
            }

            return View(dichVu);
        }

        // GET: DichVu/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: DichVu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DichVu dichVu, IFormFile HinhAnhDichVu)
        {
            if (id != dichVu.MaDichVu)
            {
                return NotFound();
            }

            // BỎ VALIDATION CÁC TRƯỜNG KHÔNG CẦN THIẾT - GIỐNG QuanLyThucDonController
            ModelState.Remove("TT_SuDungDichVus");
            ModelState.Remove("HinhAnhDichVu");
            ModelState.Remove("HinhAnhDichVu"); // Thêm dòng này để chắc chắn

            if (ModelState.IsValid)
            {
                try
                {
                    var dichVuDB = await _context.DichVus.FindAsync(id);
                    if (dichVuDB == null) return NotFound();

                    // Cập nhật thông tin
                    dichVuDB.TenDichVu = dichVu.TenDichVu;
                    dichVuDB.GiaDV = dichVu.GiaDV;
                    dichVuDB.MoTaDV = dichVu.MoTaDV;
                    dichVuDB.TrangThaiDV = dichVu.TrangThaiDV;
                    dichVuDB.GhiChu = dichVu.GhiChu;

                    // XỬ LÝ ẢNH MỚI (nếu có) - GIỐNG CÁCH XỬ LÝ TRONG QuanLyThucDonController
                    if (HinhAnhDichVu != null && HinhAnhDichVu.Length > 0)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + HinhAnhDichVu.FileName;

                        // Đường dẫn: wwwroot/assets/img/menu - DÙNG CÙNG THƯ MỤC VỚI THỰC ĐƠN
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "menu");

                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }

                        var filePath = Path.Combine(uploadDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnhDichVu.CopyToAsync(stream);
                        }

                        dichVuDB.HinhAnhDichVu = "/assets/img/menu/" + fileName;
                    }
                    // Nếu không chọn ảnh mới, giữ nguyên ảnh cũ

                    _context.Update(dichVuDB);
                    await _context.SaveChangesAsync();

                    TempData["ToastMessage"] = "Cập nhật dịch vụ thành công!";
                    TempData["ToastType"] = "success";
                    TempData["ToastTitle"] = "Thành công";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(dichVu.MaDichVu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                TempData["ToastMessage"] = "Vui lòng kiểm tra lại thông tin nhập!";
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi dữ liệu";
            }

            return View(dichVu);
        }

        // GET: DichVu/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(m => m.MaDichVu == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // POST: DichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                try
                {
                    // Xóa file ảnh (nếu có) - THÊM KIỂM TRA AN TOÀN
                    if (!string.IsNullOrEmpty(dichVu.HinhAnhDichVu))
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", dichVu.HinhAnhDichVu.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.DichVus.Remove(dichVu);
                    await _context.SaveChangesAsync();

                    TempData["ToastMessage"] = "Xóa dịch vụ thành công!";
                    TempData["ToastType"] = "success";
                    TempData["ToastTitle"] = "Thành công";
                }
                catch (DbUpdateException ex)
                {
                    // XỬ LÝ LỖI AN TOÀN HƠN
                    TempData["ToastMessage"] = "Lỗi khi xóa dịch vụ. Vui lòng thử lại!";
                    TempData["ToastType"] = "error";
                    TempData["ToastTitle"] = "Lỗi";
                }
            }
            else
            {
                TempData["ToastMessage"] = "Không tìm thấy dịch vụ để xóa!";
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DichVuExists(string id)
        {
            return _context.DichVus.Any(e => e.MaDichVu == id);
        }
    }
}