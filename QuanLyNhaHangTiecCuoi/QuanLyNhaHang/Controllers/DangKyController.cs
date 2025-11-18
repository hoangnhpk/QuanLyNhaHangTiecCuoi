using Microsoft.AspNetCore.Mvc;

namespace QuanLyNhaHang.Controllers
{
    public class DangKyController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
