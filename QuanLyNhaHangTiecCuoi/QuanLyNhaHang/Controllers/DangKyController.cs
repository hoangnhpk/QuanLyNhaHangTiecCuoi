using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using Microsoft.AspNetCore.Http;
using System;

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
                //var existing = _context.KhachHangs.FirstOrDefault(k => k.TaiKhoanKhachHang == model.TaiKhoanKhachHang);
                //if (existing != null)
                //{
                //    ViewBag.ThongBao = "Tài khoản đã tồn tại!";
                //    return View(model);
                //}

                // Tạo mã khách hàng tự động
                //string maKH = "KH" + DateTime.Now.Ticks.ToString().Substring(10);

                //var khachHang = new KhachHang
                //{
                //    MaKhachHang = maKH,
                //    TaiKhoanKhachHang = model.TaiKhoanKhachHang,
                //    MatKhauKhachHang = model.MatKhauKhachHang,
                //    TenKhachHang = model.TenKhachHang,
                //    CccdKhachHang = model.CccdKhachHang,
                //    DiaChiKhachHang = model.DiaChiKhachHang,
                //    EmailKhachHang = model.EmailKhachHang,
                //    SdtKhachHang = model.SdtKhachHang,
                //    TrangThaiKhachHang = "Hoạt động"
                //};

                //_context.KhachHangs.Add(khachHang);
                //_context.SaveChanges();

                // Lưu Session
                //HttpContext.Session.SetString("MaKhachHang", khachHang.MaKhachHang);
                //HttpContext.Session.SetString("TenKhachHang", khachHang.TenKhachHang ?? "");


                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
