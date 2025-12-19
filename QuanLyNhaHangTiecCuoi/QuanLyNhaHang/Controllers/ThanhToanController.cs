using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "QuanLy")]
    public class ThanhToanController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public ThanhToanController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("~/ThanhToan")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/ThanhToan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThanhToanViewModel>>> GetThanhToans([FromQuery] ThanhToanFilterModel filter)
        {
            var query = from d in _context.DatTiecs
                        join p in _context.PhieuThanhToans on d.MaDatTiec equals p.MaDatTiec into pd
                        from p in pd.DefaultIfEmpty()
                        where d.TienCoc > 0
                        select new { DatTiec = d, Phieu = p };

            if (!string.IsNullOrEmpty(filter.TrangThai) && filter.TrangThai != "Tất cả trạng thái")
            {
                if (filter.TrangThai == "Đã cọc 30%")
                    query = query.Where(x => x.Phieu == null && x.DatTiec.TrangThai != "huy");
                else if (filter.TrangThai == "Thanh toán hoàn toàn")
                    query = query.Where(x => x.Phieu != null);
                else if (filter.TrangThai == "Hủy")
                    query = query.Where(x => x.DatTiec.TrangThai == "huy");
            }

            var result = await query.Select(x => new ThanhToanViewModel
            {
                MaDatTiec = x.DatTiec.MaDatTiec,
                MaThanhToan = x.Phieu != null ? x.Phieu.MaPhieu : "",
                MaKhachHang = x.DatTiec.MaKhachHang,
                TongGiaTri = (decimal)((x.DatTiec.GiaBan ?? 0) * (x.DatTiec.SoBanDat ?? 0)),
                TienCoc = x.DatTiec.TienCoc ?? 0,
                TrangThai = x.DatTiec.TrangThai, 
                NgayDatCoc = x.DatTiec.NgayDatTiec,
                NgayThanhToan = x.Phieu != null ? x.Phieu.NgayThanhToan : null,
                IsCompleted = x.Phieu != null
            }).ToListAsync();

            return Ok(result);
        }

        // GET: api/ThanhToan/Detail/{id}
        [HttpGet("Detail/{maDatTiec}")]
        public async Task<ActionResult<ThanhToanDetailModel>> GetDetail(string maDatTiec)
        {
            var datTiec = await _context.DatTiecs.FindAsync(maDatTiec);
            if (datTiec == null) return NotFound();

            var phieu = await _context.PhieuThanhToans.FirstOrDefaultAsync(p => p.MaDatTiec == maDatTiec);
            decimal tongTien = (decimal)((datTiec.GiaBan ?? 0) * (datTiec.SoBanDat ?? 0));

            return new ThanhToanDetailModel
            {
                MaDatTiec = datTiec.MaDatTiec,
                MaThanhToan = phieu?.MaPhieu ?? $"TT_{datTiec.MaDatTiec}",
                MaKhachHang = datTiec.MaKhachHang,
                TongGiaTri = tongTien,
                TienCoc = datTiec.TienCoc ?? 0,
                ConLai = tongTien - (datTiec.TienCoc ?? 0),
                TrangThai = datTiec.TrangThai,
                NgayDatCoc = datTiec.NgayDatTiec,
                NgayThanhToan = phieu?.NgayThanhToan,
                PhuongThucThanhToan = phieu?.PhuongThucThanhToan,
                GhiChu = datTiec.TrangThai == "huy" ? datTiec.ChiTiet : (phieu?.GhiChu ?? ""),
                // Bổ sung NgayToChuc vào ViewModel để Frontend có thể kiểm tra
                NgayToChuc = datTiec.NgayToChuc 
            };
        }

        // POST: api/ThanhToan/Confirm
        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ThanhToanDetailModel model)
        {
            try 
            {
                var datTiec = await _context.DatTiecs.FindAsync(model.MaDatTiec);
                if (datTiec == null) return NotFound("Không tìm thấy đơn tiệc.");

                if (model.TrangThai == "Hủy")
                {
                    // RÀNG BUỘC: Chỉ được hủy trước ít nhất 3 ngày
                    if (datTiec.NgayToChuc.HasValue)
                    {
                        var daysRemaining = (datTiec.NgayToChuc.Value.Date - DateTime.Now.Date).Days;
                        if (daysRemaining < 3)
                        {
                            return BadRequest($"Không thể hủy tiệc. Tiệc diễn ra vào ngày {datTiec.NgayToChuc.Value.ToString("dd/MM/yyyy")}, chỉ được hủy trước ngày tổ chức tối thiểu 3 ngày.");
                        }
                    }

                    datTiec.TrangThai = "huy"; 
                    datTiec.ChiTiet = model.GhiChu;

                    var oldPhieu = await _context.PhieuThanhToans.FirstOrDefaultAsync(p => p.MaDatTiec == model.MaDatTiec);
                    if (oldPhieu != null) _context.PhieuThanhToans.Remove(oldPhieu);
                }
                else if (model.TrangThai == "Thanh toán hoàn toàn")
                {
                    var phieu = await _context.PhieuThanhToans.FirstOrDefaultAsync(p => p.MaDatTiec == model.MaDatTiec);
                    if (phieu == null)
                    {
                        phieu = new PhieuThanhToan
                        {
                            MaPhieu = model.MaThanhToan,
                            MaDatTiec = model.MaDatTiec,
                            NgayThanhToan = DateTime.Now,
                            PhuongThucThanhToan = model.PhuongThucThanhToan ?? "Tiền mặt",
                            TongTien = model.TongGiaTri,
                            TrangThai = "Completed",
                            GhiChu = "Đã thanh toán đủ"
                        };
                        _context.PhieuThanhToans.Add(phieu);
                    }
                    datTiec.TrangThai = "Completed";
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}