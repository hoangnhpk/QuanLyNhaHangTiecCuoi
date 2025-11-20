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
            try
            {
                // Xử lý NULL values để tránh SqlNullValueException
                var khachHangs = await _context.KhachHangs
                    .AsNoTracking()
                    .Select(kh => new KhachHang
                    {
                        MaKhachHang = kh.MaKhachHang ?? string.Empty,
                        TenKhachHang = kh.TenKhachHang ?? "Chưa có tên",
                        CccdKhachHang = kh.CccdKhachHang ?? "Chưa có CCCD",
                        SdtKhachHang = kh.SdtKhachHang ?? "Chưa có SĐT",
                        DiaChiKhachHang = kh.DiaChiKhachHang ?? "Chưa có địa chỉ",
                        EmailKhachHang = kh.EmailKhachHang ?? "Chưa có email",
                        TaiKhoanKhachHang = kh.TaiKhoanKhachHang ?? "Chưa có tài khoản",
                        MatKhauKhachHang = "********", // Ẩn mật khẩu
                        TrangThaiKhachHang = kh.TrangThaiKhachHang ?? "Active",
                        GhiChu = kh.GhiChu ?? "Không có ghi chú",
                        DatTiecs = kh.DatTiecs // Giữ nguyên navigation property
                    })
                    .ToListAsync();

                return View(khachHangs);
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Lỗi khi tải danh sách khách hàng: {ex.Message}");
                // Trả về danh sách rỗng thay vì để crash
                return View(new List<KhachHang>());
            }
        }

        // GET: QL_KhachHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
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
            return View();
        }

        // POST: QL_KhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhachHang khachHang)
        {
            // Loại bỏ validation cho DatTiecs vì đây là navigation property
            ModelState.Remove("DatTiecs");

            // Xử lý NULL values trước khi lưu (Giả định Mã khách hàng cũng không null)
            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
            khachHang.EmailKhachHang ??= string.Empty;
            khachHang.TaiKhoanKhachHang ??= string.Empty;
            khachHang.MatKhauKhachHang ??= string.Empty;
            khachHang.TrangThaiKhachHang ??= "Active";
            khachHang.GhiChu ??= string.Empty;

            // Vùng 1: KIỂM TRA TRÙNG MÃ KHI THÊM MỚI
            if (await KhachHangExistsAsync(khachHang.MaKhachHang))
            {
                ModelState.AddModelError("MaKhachHang", $"Mã khách hàng '{khachHang.MaKhachHang}' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();

                // 🔔 THÊM THÔNG BÁO THÀNH CÔNG
                TempData["SuccessMessage"] = "Thêm Khách hàng mới thành công!";

                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: QL_KhachHang/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: QL_KhachHang/Edit/5
        // POST: QL_KhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            // Tải đối tượng khách hàng HIỆN TẠI (Original) từ CSDL
            // Không dùng AsNoTracking() ở đây vì ta sẽ dùng nó để kiểm tra.
            var originalKhachHang = await _context.KhachHangs.FindAsync(id);

            if (originalKhachHang == null)
            {
                return NotFound();
            }

            // --- BẮT ĐẦU LOGIC GIỮ MẬT KHẨU CŨ (FIX) ---
            // Lấy mật khẩu mới được gửi từ form (giá trị này là "" nếu form trống)
            string newPasswordFromForm = khachHang.MatKhauKhachHang;

            if (string.IsNullOrEmpty(newPasswordFromForm))
            {
                // Nếu form gửi mật khẩu rỗng, GÁN MẬT KHẨU CŨ VÀO ĐỐI TƯỢNG ĐANG CẬP NHẬT
                khachHang.MatKhauKhachHang = originalKhachHang.MatKhauKhachHang;
            }
            else
            {
                // Nếu có mật khẩu mới, dùng mật khẩu mới này (thường cần hash ở đây)
                khachHang.MatKhauKhachHang = newPasswordFromForm;
            }
            // --- KẾT THÚC LOGIC GIỮ MẬT KHẨU CŨ (FIX) ---


            // Loại bỏ validation cho DatTiecs vì đây là navigation property
            ModelState.Remove("DatTiecs");

            // Xử lý NULL values cho các trường khác (Giữ nguyên)
            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
            khachHang.EmailKhachHang ??= string.Empty;
            khachHang.TaiKhoanKhachHang ??= string.Empty;
            khachHang.TrangThaiKhachHang ??= "Active";
            khachHang.GhiChu ??= string.Empty;

            // Vùng 2: KIỂM TRA TRÙNG MÃ KHI CẬP NHẬT
            if (id != khachHang.MaKhachHang && await KhachHangExistsAsync(khachHang.MaKhachHang))
            {
                ModelState.AddModelError("MaKhachHang", $"Mã khách hàng '{khachHang.MaKhachHang}' đã tồn tại trong CSDL. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Thay vì dùng _context.Update(khachHang), chúng ta dùng Copy Properties:
                    // Phương pháp này đảm bảo EF Core chỉ cập nhật những trường cần thiết.

                    // 1. Tải đối tượng gốc vào tracking (đã làm ở trên: originalKhachHang)
                    // 2. Gán các giá trị mới từ form vào đối tượng gốc đã được theo dõi
                    originalKhachHang.TenKhachHang = khachHang.TenKhachHang;
                    originalKhachHang.CccdKhachHang = khachHang.CccdKhachHang;
                    originalKhachHang.SdtKhachHang = khachHang.SdtKhachHang;
                    originalKhachHang.DiaChiKhachHang = khachHang.DiaChiKhachHang;
                    originalKhachHang.EmailKhachHang = khachHang.EmailKhachHang;
                    originalKhachHang.TaiKhoanKhachHang = khachHang.TaiKhoanKhachHang;
                    originalKhachHang.TrangThaiKhachHang = khachHang.TrangThaiKhachHang;
                    originalKhachHang.GhiChu = khachHang.GhiChu;

                    // QUAN TRỌNG: Gán mật khẩu đã được xử lý (hoặc mới, hoặc cũ)
                    originalKhachHang.MatKhauKhachHang = khachHang.MatKhauKhachHang;

                    // SaveChanges sẽ chỉ cập nhật các trường đã thay đổi trong đối tượng originalKhachHang
                    await _context.SaveChangesAsync();
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

                // 🔔 THÊM THÔNG BÁO THÀNH CÔNG
                TempData["SuccessMessage"] = $"Cập nhật Khách hàng '{khachHang.MaKhachHang}' thành công!";

                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, trả về view (và lúc này khachHang.MatKhauKhachHang đã chứa mật khẩu cũ hoặc mới nhập)
            return View(khachHang);
        }

        // GET: QL_KhachHang/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Console.WriteLine("------------------------------" + id);
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
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
            Console.WriteLine("------------------------------" + id);
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();

                // 🔔 THÊM THÔNG BÁO THÀNH CÔNG
                TempData["SuccessMessage"] = $"Xóa Khách hàng có mã '{id}' thành công!";
            }
            else
            {
                // 🔔 THÊM THÔNG BÁO LỖI
                TempData["ErrorMessage"] = $"Không tìm thấy Khách hàng có mã '{id}' để xóa.";
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================================================================
        // 🔒 ACTION KHÓA TÀI KHOẢN (GIỮ NGUYÊN LOGIC GỐC)
        // =========================================================================
        [HttpGet] // Sử dụng GET vì nó được gọi từ thẻ <a> (sau khi xác nhận JS)
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã khách hàng để khóa.";
                return RedirectToAction(nameof(Index));
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                TempData["ErrorMessage"] = $"Không tìm thấy khách hàng có mã '{id}'.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra trạng thái hiện tại
            if (khachHang.TrangThaiKhachHang == "Active")
            {
                // Thực hiện chuyển trạng thái từ Active -> Inactive
                khachHang.TrangThaiKhachHang = "Inactive";

                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();

                    // 🔔 THÔNG BÁO THÀNH CÔNG
                    TempData["SuccessMessage"] = $"Khóa tài khoản khách hàng '{khachHang.TenKhachHang}' (Mã: {id}) thành công! Trạng thái đã chuyển sang Ngừng Hoạt Động.";
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu có lỗi trong quá trình cập nhật
                    Console.WriteLine($"Lỗi khi khóa tài khoản khách hàng '{id}': {ex.Message}");
                    TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi khóa tài khoản khách hàng '{khachHang.TenKhachHang}'.";
                }
            }
            else
            {
                // Nếu tài khoản đã Inactive hoặc ở trạng thái khác
                TempData["ErrorMessage"] = $"Tài khoản khách hàng '{khachHang.TenKhachHang}' hiện đang ở trạng thái '{khachHang.TrangThaiKhachHang}'. Không cần khóa.";
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================================================================
        // PHƯƠNG THỨC HỖ TRỢ (ĐÃ KHẮC PHỤC LỖI THIẾU)
        // =========================================================================
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