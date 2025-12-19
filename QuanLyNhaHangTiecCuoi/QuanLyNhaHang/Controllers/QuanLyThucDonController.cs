using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class QuanLyThucDonController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public QuanLyThucDonController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string category)
        {
            // 1. Lấy danh sách Combo và nạp kèm các món ăn thành phần
            var listCombo = await _context.ComboMons
                                          .Include(c => c.ChiTietCombos)
                                          .ThenInclude(ct => ct.MonAn)
                                          .OrderByDescending(c => c.NgayTaoCombo)
                                          .ToListAsync();

            // 2. LỌC DỮ LIỆU: Chỉ giữ lại các chi tiết của món ăn ĐANG BÁN
            // Nếu món ăn đã bị xóa/ẩn (Ngừng bán), nó sẽ không hiện trong danh sách món của Combo nữa
            foreach (var cb in listCombo)
            {
                cb.ChiTietCombos = cb.ChiTietCombos
                                     .Where(ct => ct.MonAn != null && ct.MonAn.TrangThaiMonAn != "Ngừng bán")
                                     .ToList();
            }
            ViewBag.ListCombo = listCombo;

            // 3. Lấy danh sách món lẻ bên phải (chỉ lấy món chưa bị ẩn)
            var query = _context.MonAns.AsQueryable();
            query = query.Where(m => m.TrangThaiMonAn != "Ngừng bán");

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.TenMonAn.Contains(searchString) || m.MaMonAn.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(m => m.LoaiMonAn == category);
            }

            var listMonAn = await query.OrderBy(m => m.MaMonAn).ToListAsync();
            return View(listMonAn);
        }
        public async Task<IActionResult> ThemCombo()
        {
            // Lấy danh sách món ăn để hiển thị ra checkbox
            ViewBag.ListMonAn = await _context.MonAns
                                      .OrderBy(m => m.TenMonAn)
                                      .ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemCombo(ComboMon model, List<string> monAnChon, IFormFile imageFile)
        {
            ModelState.Remove("ChiTietCombos");
            ModelState.Remove("HinhAnhCombo");
            ModelState.Remove("imageFile");
            if (imageFile == null || imageFile.Length == 0)
            {
                // Thêm lỗi vào danh sách để hiện ra khung đỏ
                ModelState.AddModelError("imageFile", "Vui lòng chọn hình ảnh cho Combo!");
            }

            if (monAnChon == null || monAnChon.Count == 0)
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một món ăn!");

            if (await _context.ComboMons.AnyAsync(x => x.MaComboMon == model.MaComboMon))
                ModelState.AddModelError("MaComboMon", "Mã Combo này đã tồn tại!");

            if (ModelState.IsValid)
            {
                // 1. XỬ LÝ UPLOAD ẢNH (Sửa đường dẫn tại đây)
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Tạo tên file độc nhất
                    var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + imageFile.FileName;


                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "menu");

                    // Nếu thư mục chưa có thì tự tạo
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                 
                    model.HinhAnhCombo = "/assets/img/menu/" + fileName;
                }

                // 2. Lưu Combo
                model.NgayTaoCombo = DateTime.Now;
                _context.ComboMons.Add(model);
                await _context.SaveChangesAsync();

                // 3. Lưu chi tiết
                if (monAnChon != null)
                {
                    foreach (var maMon in monAnChon)
                    {
                        int sl = 1;
                        string keySl = $"soLuong_{maMon}";
                        if (Request.Form.ContainsKey(keySl)) int.TryParse(Request.Form[keySl], out sl);

                        var ct = new ChiTietCombo
                        {
                            MaChiTietCombo = Guid.NewGuid().ToString().Substring(0, 15),
                            MaComboMon = model.MaComboMon,
                            MaMonAn = maMon,
                            SoLuong = sl > 0 ? sl : 1
                        };
                        _context.ChiTietCombos.Add(ct);
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Thêm combo thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListMonAn = await _context.MonAns.OrderBy(m => m.TenMonAn).ToListAsync();
            return View(model);
        }
        public async Task<IActionResult> SuaCombo(string id)
{
    if (id == null) return NotFound();

    // 1. Tìm Combo và Include luôn danh sách chi tiết món
    var combo = await _context.ComboMons
                              .Include(c => c.ChiTietCombos) 
                              .FirstOrDefaultAsync(m => m.MaComboMon == id);

    if (combo == null) return NotFound();

    ViewBag.ListMonAn = await _context.MonAns
                              .OrderBy(m => m.TenMonAn)
                              .ToListAsync();

    return View(combo);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaCombo(string id, ComboMon model, List<string> monAnChon, IFormFile imageFile)
        {
            ModelState.Remove("ChiTietCombos");
            ModelState.Remove("HinhAnhCombo");
            ModelState.Remove("imageFile");
           

            if (id != model.MaComboMon) return NotFound();

            if (monAnChon == null || monAnChon.Count == 0)
                ModelState.AddModelError("", "Combo phải có ít nhất một món ăn!");

            if (ModelState.IsValid)
            {
                try
                {
                    var comboDB = await _context.ComboMons.FindAsync(id);
                    if (comboDB == null) return NotFound();

                    // Update thông tin
                    comboDB.TenCombo = model.TenCombo;
                    comboDB.GiaCombo = model.GiaCombo;
                    comboDB.SoLuong = model.SoLuong;
                    comboDB.TrangThai = model.TrangThai;
                    comboDB.MoTa = model.MoTa;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + imageFile.FileName;
                        // Đường dẫn thư mục: wwwroot/assets/img/combos
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "menu");
                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        var filePath = Path.Combine(uploadDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn mới vào DB
                        comboDB.HinhAnhCombo = "/assets/img/menu/" + fileName;
                    }

                    _context.Update(comboDB);

                    // 2. Xử lý chi tiết
                    var oldDetails = _context.ChiTietCombos.Where(d => d.MaComboMon == id);
                    _context.ChiTietCombos.RemoveRange(oldDetails);

                    if (monAnChon != null)
                    {
                        foreach (var maMon in monAnChon)
                        {
                            int sl = 1;
                            string keySl = $"soLuong_{maMon}";
                            if (Request.Form.ContainsKey(keySl)) int.TryParse(Request.Form[keySl], out sl);

                            var ct = new ChiTietCombo
                            {
                                MaChiTietCombo = Guid.NewGuid().ToString().Substring(0, 15),
                                MaComboMon = id,
                                MaMonAn = maMon,
                                SoLuong = sl > 0 ? sl : 1
                            };
                            _context.ChiTietCombos.Add(ct);
                        }
                    }
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật combo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ComboMons.AnyAsync(e => e.MaComboMon == id)) return NotFound();
                    else throw;
                }
            }

            ViewBag.ListMonAn = await _context.MonAns.OrderBy(m => m.TenMonAn).ToListAsync();
            return View(model);
        }

        // --- XÓA COMBO ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaCombo(string id)
        {
            var combo = await _context.ComboMons.FindAsync(id);
            if (combo != null)
            {
                var chiTiets = _context.ChiTietCombos.Where(x => x.MaComboMon == id);
                _context.ChiTietCombos.RemoveRange(chiTiets);

                _context.ComboMons.Remove(combo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa combo!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy combo!";
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Them() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(MonAn model, IFormFile imageFile)
        {
            // Bỏ qua các lỗi validation mặc định không cần thiết
            ModelState.Remove("HinhAnhMonAn"); 
            ModelState.Remove("imageFile");
            if (imageFile == null || imageFile.Length == 0)
            {
                // Thêm lỗi vào danh sách để hiện ra khung đỏ
                ModelState.AddModelError("imageFile", "Vui lòng chọn hình ảnh cho Combo!");
            }

            // 1. Kiểm tra bắt buộc chọn ảnh
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("imageFile", "Vui lòng chọn hình ảnh món ăn!");
            }

            // 2. Kiểm tra trùng mã
            if (await _context.MonAns.AnyAsync(x => x.MaMonAn == model.MaMonAn))
            {
                ModelState.AddModelError("MaMonAn", "Mã món ăn đã tồn tại!");
            }

            if (ModelState.IsValid)
            {
                // 3. Xử lý lưu ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + imageFile.FileName;

                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "menu");

                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    var filePath = Path.Combine(uploadDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn vào DB
                    model.HinhAnhMonAn = "/assets/img/menu/" + fileName;
                }

                _context.MonAns.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm món ăn thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Sua(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var item = await _context.MonAns.FindAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(MonAn model, IFormFile imageFile)
        {
            // Bỏ qua check ảnh vì sửa không bắt buộc chọn mới
            ModelState.Remove("HinhAnhMonAn");
            ModelState.Remove("imageFile");
           

            if (ModelState.IsValid)
            {
                var item = await _context.MonAns.FindAsync(model.MaMonAn);
                if (item != null)
                {
                    // Cập nhật thông tin
                    item.TenMonAn = model.TenMonAn;
                    item.DonViTinh = model.DonViTinh;
                    item.DonGia = model.DonGia;
                    item.LoaiMonAn = model.LoaiMonAn;
                    item.TrangThaiMonAn = model.TrangThaiMonAn;
                    item.MoTaMonAn = model.MoTaMonAn;
                    item.GhiChu = model.GhiChu;

                    // 4. Xử lý ảnh mới (nếu có chọn)
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + imageFile.FileName;
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "menu");
                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        var filePath = Path.Combine(uploadDir, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn mới
                        item.HinhAnhMonAn = "/assets/img/menu/" + fileName;
                    }
                    // Nếu không chọn ảnh, giữ nguyên item.HinhAnhMonAn cũ

                    _context.Update(item);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật món thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(string id)
        {
            var item = await _context.MonAns.FindAsync(id);
            if (item == null) return NotFound();

            try
            {
                // Bước 1: Tìm và xóa sạch mọi liên kết của món này trong tất cả các Combo
                var relatedDetails = _context.ChiTietCombos.Where(ct => ct.MaMonAn == id);
                _context.ChiTietCombos.RemoveRange(relatedDetails);
                await _context.SaveChangesAsync();

                // Bước 2: Thử xóa món ăn khỏi database
                _context.MonAns.Remove(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa món ăn và gỡ khỏi mọi combo thành công!";
            }
            catch (Exception)
            {
                // Bước 3: Nếu món đã nằm trong Hóa đơn cũ (không được xóa vật lý), hãy ẩn nó đi
                var itemToHide = await _context.MonAns.FindAsync(id);
                if (itemToHide != null)
                {
                    itemToHide.TrangThaiMonAn = "Ngừng bán";
                    _context.Update(itemToHide);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Món ăn đã được ẩn khỏi danh sách và các combo liên quan!";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}