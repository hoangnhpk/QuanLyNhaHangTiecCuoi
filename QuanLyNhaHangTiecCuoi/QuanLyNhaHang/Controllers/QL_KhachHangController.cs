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
                // 🔔 SỬ DỤNG .Include(kh => kh.TaiKhoan) để tải thông tin tài khoản
                var khachHangs = await _context.KhachHangs
                    .Include(kh => kh.TaiKhoan) // <--- Thêm .Include() ở đây
                    .AsNoTracking()
                    .Select(kh => new KhachHang
                    {
                        MaKhachHang = kh.MaKhachHang ?? string.Empty,
                        TenKhachHang = kh.TenKhachHang ?? "Chưa có tên",
                        CccdKhachHang = kh.CccdKhachHang ?? "Chưa có CCCD",
                        SdtKhachHang = kh.SdtKhachHang ?? "Chưa có SĐT",
                        DiaChiKhachHang = kh.DiaChiKhachHang ?? "Chưa có địa chỉ",
                        TrangThaiKhachHang = kh.TrangThaiKhachHang ?? "Active",
                        GhiChu = kh.GhiChu ?? "Không có ghi chú",
                        DatTiecs = kh.DatTiecs, // Giữ nguyên navigation property

                        // LẤY THÔNG TIN TỪ BẢNG TAIKHOAN qua Navigation Property
                        // Ta có thể truy cập các thuộc tính của TaiKhoan qua kh.TaiKhoan
                        MaTaiKhoan = kh.MaTaiKhoan, // Mã tài khoản
                        TaiKhoan = kh.TaiKhoan // Toàn bộ đối tượng TaiKhoan (cho View truy cập)

                        // ⚠️ CÁC TRƯỜNG BỊ LOẠI BỎ (KHÔNG CÓ TRONG MODEL):
                        // EmailKhachHang, TaiKhoanKhachHang, MatKhauKhachHang
                    })
                    .ToListAsync();

                return View(khachHangs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải danh sách khách hàng: {ex.Message}");
                return View(new List<KhachHang>());
            }
        }

        // --- Details, Edit, Delete cũng cần Include nếu muốn dùng TaiKhoan trong View ---

        // GET: QL_KhachHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(kh => kh.TaiKhoan) // <--- Thêm .Include() ở đây
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
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

            // 💡 Khi FindAsync, EF Core chỉ tải đối tượng chính, không tải Navigation Property.
            // Nếu bạn muốn hiển thị thông tin TaiKhoan trong View Edit, bạn cần dùng FirstOrDefaultAsync có Include
            var khachHang = await _context.KhachHangs
                                .Include(kh => kh.TaiKhoan)
                                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }


        // POST: QL_KhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            // Tải đối tượng khách hàng HIỆN TẠI (Original) từ CSDL, CÓ KÈM TaiKhoan
            var originalKhachHang = await _context.KhachHangs
                .Include(kh => kh.TaiKhoan) // <--- Cần Include để truy cập mật khẩu cũ
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (originalKhachHang == null)
            {
                return NotFound();
            }

            // --- BẮT ĐẦU LOGIC GIỮ MẬT KHẨU CŨ (FIX) ---
            // ⚠️ LƯU Ý: MẬT KHẨU nằm trong bảng TaiKhoan (kh.TaiKhoan.Password).
            // Logic này sẽ không hoạt động nếu bạn không cập nhật cả đối tượng TaiKhoan.
            // Để đơn giản, tôi giả định bạn đang cập nhật Password trong bảng TaiKhoan qua đối tượng TaiKhoan.
            // Nếu MatKhauKhachHang là một trường cũ trong KhachHang, logic dưới đây sẽ không đúng nữa.

            // Giả định bạn đã loại bỏ các trường mật khẩu cũ không dùng nữa (MatKhauKhachHang,...)
            // Nếu bạn muốn cập nhật mật khẩu, bạn phải cập nhật đối tượng TaiKhoan liên quan.

            /* // LOGIC DƯỚI ĐÂY BỊ LOẠI BỎ VÌ KHÔNG CÓ TRƯỜNG MatKhauKhachHang TRONG MODEL KHÁCH HÀNG:
            string newPasswordFromForm = khachHang.MatKhauKhachHang;

            if (string.IsNullOrEmpty(newPasswordFromForm))
            {
                // Giữ mật khẩu cũ
                khachHang.MatKhauKhachHang = originalKhachHang.MatKhauKhachHang; 
            }
            else
            {
                khachHang.MatKhauKhachHang = newPasswordFromForm;
            }
            */
            // --- KẾT THÚC LOGIC GIỮ MẬT KHẨU CŨ (FIX) ---


            // Loại bỏ validation cho DatTiecs và TaiKhoan
            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan"); // Loại bỏ TaiKhoan vì nó là Navigation Property

            // Xử lý NULL values cho các trường khác (Giữ nguyên)
            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
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
                    // 1. Gán các giá trị mới từ form vào đối tượng gốc đã được theo dõi
                    originalKhachHang.TenKhachHang = khachHang.TenKhachHang;
                    originalKhachHang.CccdKhachHang = khachHang.CccdKhachHang;
                    originalKhachHang.SdtKhachHang = khachHang.SdtKhachHang;
                    originalKhachHang.DiaChiKhachHang = khachHang.DiaChiKhachHang;
                    originalKhachHang.TrangThaiKhachHang = khachHang.TrangThaiKhachHang;
                    originalKhachHang.GhiChu = khachHang.GhiChu;

                    // ⚠️ CHÚ Ý: Các trường Email, Username, Password, Vaitro nằm trong bảng TaiKhoan. 
                    // Để cập nhật chúng, bạn cần truyền và cập nhật riêng TaiKhoan object.
                    // Hiện tại, Controller này chỉ cập nhật KhachHang.

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
            return View(khachHang);
        }

        // --- Các Action Create, Delete, Lock không cần thay đổi logic truy vấn CSDL ---

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
        public async Task<IActionResult> Create(KhachHang khachHang)
        {
            // Loại bỏ validation cho DatTiecs và TaiKhoan
            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan");

            // Xử lý NULL values trước khi lưu
            // LOẠI BỎ CÁC TRƯỜNG EMAILKHACHHANG, TAIKHOANKHACHHANG, MATKHAUKHACHHANG 
            // KHÔNG CÓ TRONG MODEL KHÁCH HÀNG

            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
            khachHang.TrangThaiKhachHang ??= "Active";
            khachHang.GhiChu ??= string.Empty;

            // Xử lý MaTaiKhoan (nếu không có thì gán null hoặc tạo mới TaiKhoan)
            // Hiện tại ta chỉ đảm bảo nó không gây lỗi khi lưu
            khachHang.MaTaiKhoan ??= null;

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

            // 💡 Cung cấp lại danh sách MaTaiKhoan nếu ModelState không hợp lệ
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan", khachHang.MaTaiKhoan);
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
                .Include(kh => kh.TaiKhoan) // <--- Thêm .Include() ở đây
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
        // PHƯƠNG THỨC HỖ TRỢ (GIỮ NGUYÊN)
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