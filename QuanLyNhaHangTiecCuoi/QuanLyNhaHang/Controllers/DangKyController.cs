using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace QuanLyNhaHang.Controllers
{
    public class DangKyController : Controller
    {
        private readonly QuanLyNhaHangContext _context;
        public DangKyController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(KhachHangDangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng tài khoản
                var existing = _context.TaiKhoans.FirstOrDefault(k => k.UserName == model.TaiKhoanKhachHang);
                if (existing != null)
                {
                    ViewBag.ThongBao = "Tài khoản đã tồn tại!";
                    return View(model);
                }

                // Tạo mã tài khoản và khách hàng
                string maTK = "TK" + DateTime.Now.Ticks.ToString().Substring(10);
                string maKH = "KH" + DateTime.Now.Ticks.ToString().Substring(10);

                // Tạo tài khoản
                var taiKhoan = new TaiKhoan
                {
                    MaTaiKhoan = maTK,
                    UserName = model.TaiKhoanKhachHang,
                    Password = model.MatKhauKhachHang, // TODO: Hash mật khẩu
                    Email = model.EmailKhachHang,
                    VaiTro = "Khách hàng",
                    TrangThai = "Hoạt động"
                };
                _context.TaiKhoans.Add(taiKhoan);

                // Tạo khách hàng, gán MaTaiKhoan
                var khachHang = new KhachHang
                {
                    MaKhachHang = maKH,
                    MaTaiKhoan = maTK,
                    TenKhachHang = model.TenKhachHang,
                    CccdKhachHang = model.CccdKhachHang,
                    DiaChiKhachHang = model.DiaChiKhachHang,
                    SdtKhachHang = model.SdtKhachHang,
                    TrangThaiKhachHang = "Hoạt động"
                };
                _context.KhachHangs.Add(khachHang);

                // Lưu thay đổi
                _context.SaveChanges();

                // Lưu Session
                HttpContext.Session.SetString("MaKhachHang", khachHang.MaKhachHang);
                HttpContext.Session.SetString("TenKhachHang", khachHang.TenKhachHang ?? "");

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
