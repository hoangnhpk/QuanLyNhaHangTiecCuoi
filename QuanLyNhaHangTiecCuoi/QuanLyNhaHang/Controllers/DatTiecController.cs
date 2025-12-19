using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Models.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyNhaHang.Controllers
{
    public class DatTiecController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public DatTiecController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public class DichVuTuChonItem
        {
            public string MaDichVu { get; set; }
            public int SoLuong { get; set; }
        }

        [HttpPost]
        public IActionResult DatTiec(DatTiecVM model)
        {
            var maKhachHang = HttpContext.Session.GetString("MaKhachHang");
            if (string.IsNullOrEmpty(maKhachHang)) return RedirectToAction("Index", "DangNhap");

            if (ModelState.IsValid)
            {
                if (model.NgayToChuc.HasValue)
                {
                    bool daCoTiec = _context.DatTiecs.Any(d =>
                        d.NgayToChuc.HasValue &&
                        d.NgayToChuc.Value.Date == model.NgayToChuc.Value.Date &&
                        d.TrangThai != "Hủy"
                    );

                    if (daCoTiec)
                    {
                        TempData["ErrorMessage"] = $"Rất tiếc, ngày {model.NgayToChuc.Value:dd/MM/yyyy} nhà hàng đã có tiệc.";
                        return Redirect(Url.Action("Index", "Home") + "#book-a-table");
                    }
                }
                try
                {
                    decimal giaThucDonMoiBan = 0;
                    decimal giaDichVuTotal = 0;

                    // A. Tính tiền Thực đơn
                    if (model.LoaiThucDon == "TuChon" && !string.IsNullOrEmpty(model.ThucDonTuChonJson))
                    {
                        var danhSachMon = JsonConvert.DeserializeObject<List<MonTuChonItem>>(model.ThucDonTuChonJson);
                        if (danhSachMon != null)
                        {
                            foreach (var item in danhSachMon)
                            {
                                if (string.IsNullOrEmpty(item.MaMonAn)) continue;
                                var monDb = _context.MonAns.Find(item.MaMonAn);
                                // FIX: Xóa .HasValue và .Value
                                if (monDb != null)
                                {
                                    giaThucDonMoiBan += (monDb.DonGia ?? 0) * item.SoLuong;
                                }
                            }
                        }
                    }
                    else if (model.LoaiThucDon == "Vàng") giaThucDonMoiBan = 1000000;
                    else if (model.LoaiThucDon == "Bạch Kim") giaThucDonMoiBan = 2000000;

                    // B. Tính tiền Dịch vụ
                    if (model.LoaiDichVu == "TuChon" && !string.IsNullOrEmpty(model.DichVuTuChonJson))
                    {
                        var danhSachDV = JsonConvert.DeserializeObject<List<DichVuTuChonItem>>(model.DichVuTuChonJson);
                        if (danhSachDV != null)
                        {
                            foreach (var item in danhSachDV)
                            {
                                var dvDb = _context.DichVus.Find(item.MaDichVu);
                                // FIX: Xóa .HasValue và .Value
                                if (dvDb != null)
                                {
                                    giaDichVuTotal += (dvDb.GiaDV * item.SoLuong);
                                }
                            }
                        }
                    }
                    else if (model.LoaiDichVu == "Basic") giaDichVuTotal = 15000000;
                    else if (model.LoaiDichVu == "VIP") giaDichVuTotal = 50000000;

                    int soBan = model.SoBanDat ?? 0;
                    decimal tongTienHopDong = (giaThucDonMoiBan * soBan) + giaDichVuTotal;
                    decimal tienCocPhaiDong = tongTienHopDong * 0.3m;

                    var tiec = new DatTiec
                    {
                        MaDatTiec = PhatSinhMaTiec(),
                        MaKhachHang = maKhachHang,
                        TenCoDau = model.TenCoDau,
                        TenChuRe = model.TenChuRe,
                        NgayToChuc = model.NgayToChuc,
                        GioToChuc = model.GioToChuc,
                        SoBanDat = soBan,
                        NgayDatTiec = DateTime.Now,
                        TrangThai = "Mới đặt",
                        GiaBan = tongTienHopDong,
                        TienCoc = tienCocPhaiDong,
                        ChiTiet = $"Dự phòng: {model.SoBanDuPhong} bàn. Ghi chú: {model.GhiChu}"
                    };

                    _context.DatTiecs.Add(tiec);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = $"Đặt tiệc thành công! Vui lòng thanh toán cọc: {tienCocPhaiDong:N0} VNĐ";
                    return RedirectToAction("ThanhToanCoc", new { maDatTiec = tiec.MaDatTiec });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private string PhatSinhMaTiec()
        {
            var lastItem = _context.DatTiecs.OrderByDescending(x => x.MaDatTiec).FirstOrDefault();
            if (lastItem == null) return "DT001";
            int nextId = int.Parse(lastItem.MaDatTiec.Substring(2)) + 1;
            return "DT" + nextId.ToString("D3");
        }

        [HttpGet]
        public IActionResult GetThucDonTuChon()
        {
            var danhSachNhom = _context.MonAns.Where(m => m.TrangThaiMonAn != "Ngừng phục vụ").GroupBy(m => m.LoaiMonAn).ToList();
            return PartialView("_ThucDonTuChon", danhSachNhom);
        }

        [HttpGet]
        public IActionResult GetDichVuTuChon()
        {
            var danhSachDichVu = _context.DichVus.Where(dv => dv.TrangThaiDV != "Ngừng phục vụ").ToList();
            return PartialView("_DichVuTuChon", danhSachDichVu);
        }

        public class MonTuChonItem
        {
            public string MaMonAn { get; set; }
            public int SoLuong { get; set; }
        }

        [HttpGet]
        public IActionResult ThanhToanCoc(string maDatTiec)
        {
            var tiec = _context.DatTiecs.Include(t => t.KhachHang).FirstOrDefault(t => t.MaDatTiec == maDatTiec);
            return View(tiec);
        }

        [HttpPost]
        public IActionResult XacNhanDaCoc(string maDatTiec)
        {
            var tiec = _context.DatTiecs.Find(maDatTiec);
            if (tiec != null) { tiec.TrangThai = "Chờ duyệt (Đã chuyển khoản)"; _context.SaveChanges(); }
            return RedirectToAction("Index", "Home");
        }
    }
}