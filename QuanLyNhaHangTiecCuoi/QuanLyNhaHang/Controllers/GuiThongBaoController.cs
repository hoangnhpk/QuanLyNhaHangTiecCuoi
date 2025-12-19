using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Services;
using QuanLyNhaHang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace QuanLyNhaHang.Controllers
{
    public class GuiThongBaoController : Controller
    {
        private readonly IEmailService _emailService;

        // Inject EmailService vào Controller
        public GuiThongBaoController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // 1. Hiển thị form soạn thảo
        public IActionResult SoanEmail()
        {
            return View();
        }

        // 2. Xử lý gửi Email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoanEmail(GuiEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Trang trí nội dung HTML một chút cho đẹp
                    string noiDungDep = $@"
                        <div style='font-family:Arial, sans-serif; padding:20px; border:1px solid #ddd;'>
                            <h2 style='color:#d35400;'>NHÀ HÀNG LONG PHỤNG TRÂN TRỌNG THÔNG BÁO</h2>
                            <hr />
                            <p>Kính gửi quý khách,</p>
                            <p>{model.NoiDung.Replace("\n", "<br>")}</p> 
                            <br />
                            <p><i>Trân trọng,<br>Nhà Hàng Tiệc Cưới Long Phụng.</i></p>
                        </div>
                    ";

                    // Gọi Service gửi mail
                    _emailService.GuiEmail(model.EmailNguoiNhan, model.TieuDe, noiDungDep);

                    TempData["SuccessMessage"] = "Đã gửi thành công tới " + model.EmailNguoiNhan;

                    // Reset form sau khi gửi thành công
                    return RedirectToAction(nameof(SoanEmail));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi gửi mail: " + ex.Message);
                }
            }
            return View(model);
        }
    }
}