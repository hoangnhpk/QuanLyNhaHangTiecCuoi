using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;

namespace QuanLyNhaHang.Controllers
{
    // Định tuyến mặc định cho API là: domain/api/ThanhToan
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

        // --- 1. ACTION TRẢ VỀ GIAO DIỆN (VIEW) ---

        // Dấu "~" giúp ghi đè route của class. 
        // URL sẽ là: https://localhost:port/ThanhToan
        [HttpGet]
        [Route("~/ThanhToan")]
        [Route("~/ThanhToan/Index")]
        public IActionResult Index()
        {
            return View();
        }

        // --- 2. CÁC API XỬ LÝ DỮ LIỆU ---

        // GET: api/ThanhToan (Lấy danh sách)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThanhToanViewModel>>> GetThanhToans([FromQuery] ThanhToanFilterModel filter)
        {
            // Join DatTiec và PhieuThanhToan (Left Join vì có thể chưa có phiếu thanh toán)
            var query = from d in _context.DatTiecs
                        join p in _context.PhieuThanhToans on d.MaDatTiec equals p.MaDatTiec into pd
                        from p in pd.DefaultIfEmpty()
                            // Chỉ lấy những đơn đã đặt cọc (TienCoc > 0)
                        where d.TienCoc > 0
                        select new { DatTiec = d, Phieu = p };

            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(filter.TrangThai) && filter.TrangThai != "Tất cả trạng thái")
            {
                if (filter.TrangThai == "Đã cọc 30%")
                    query = query.Where(x => x.Phieu == null); // Chưa có phiếu thanh toán cuối
                else if (filter.TrangThai == "Thanh toán hoàn toàn")
                    query = query.Where(x => x.Phieu != null);
            }

            // Lọc theo ngày (Ngày đặt cọc hoặc ngày thanh toán)
            if (filter.FromDate.HasValue)
            {
                var searchDate = filter.FromDate.Value.Date; // Lấy ngày (bỏ giờ phút)

                // Logic: Tìm những đơn mà (Ngày cọc trùng ngày tìm) HOẶC (Ngày thanh toán trùng ngày tìm)
                query = query.Where(x =>
                    (x.DatTiec.NgayDatTiec.HasValue && x.DatTiec.NgayDatTiec.Value.Date == searchDate)
                    ||
                    (x.Phieu != null && x.Phieu.NgayThanhToan.HasValue && x.Phieu.NgayThanhToan.Value.Date == searchDate)
                );
            }

            var result = await query.Select(x => new ThanhToanViewModel
            {
                MaDatTiec = x.DatTiec.MaDatTiec,
                MaThanhToan = x.Phieu != null ? x.Phieu.MaPhieu : "", // Chưa thanh toán thì rỗng
                MaKhachHang = x.DatTiec.MaKhachHang,
                // Giả định tổng giá trị = (Giá bàn * Số bàn). Cần điều chỉnh theo logic thực tế của bạn
                TongGiaTri = (decimal)(x.DatTiec.GiaBan * x.DatTiec.SoBanDat),
                TienCoc = x.DatTiec.TienCoc ?? 0,
                TrangThai = x.Phieu != null ? "Thanh toán hoàn toàn" : "Đã cọc 30%",
                NgayDatCoc = x.DatTiec.NgayDatTiec,
                NgayThanhToan = x.Phieu != null ? x.Phieu.NgayThanhToan : null,
                IsCompleted = x.Phieu != null
            }).ToListAsync();

            return Ok(result);
        }

        // GET: api/ThanhToan/Detail/TIEC_001 (Lấy chi tiết)
        [HttpGet("Detail/{maDatTiec}")]
        public async Task<ActionResult<ThanhToanDetailModel>> GetDetail(string maDatTiec)
        {
            var datTiec = await _context.DatTiecs.FindAsync(maDatTiec);
            if (datTiec == null) return NotFound();

            var phieu = await _context.PhieuThanhToans.FirstOrDefaultAsync(p => p.MaDatTiec == maDatTiec);

            decimal tongTien = (decimal)(datTiec.GiaBan * datTiec.SoBanDat); // Logic tính tổng tiền

            return new ThanhToanDetailModel
            {
                MaDatTiec = datTiec.MaDatTiec,
                MaThanhToan = phieu?.MaPhieu ?? $"TT_{datTiec.MaDatTiec}", // Tự generate mã hiển thị nếu chưa có
                MaKhachHang = datTiec.MaKhachHang,
                TongGiaTri = tongTien,
                TienCoc = datTiec.TienCoc ?? 0,
                ConLai = tongTien - (datTiec.TienCoc ?? 0),
                TrangThai = phieu != null ? "Thanh toán hoàn toàn" : "Đã cọc 30%",
                NgayDatCoc = datTiec.NgayDatTiec,
                NgayThanhToan = phieu?.NgayThanhToan,
                PhuongThucThanhToan = phieu?.PhuongThucThanhToan,
                GhiChu = phieu?.GhiChu
            };
        }

        // POST: api/ThanhToan/Confirm (Xác nhận thanh toán)
        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ThanhToanDetailModel model)
        {
            var datTiec = await _context.DatTiecs.FindAsync(model.MaDatTiec);
            if (datTiec == null) return NotFound("Không tìm thấy tiệc.");

            // Kiểm tra đã thanh toán chưa
            var existingPhieu = await _context.PhieuThanhToans.FirstOrDefaultAsync(p => p.MaDatTiec == model.MaDatTiec);
            if (existingPhieu != null) return BadRequest("Đơn này đã thanh toán hoàn tất rồi.");

            // Tạo phiếu thanh toán mới
            var phieu = new PhieuThanhToan
            {
                MaPhieu = model.MaThanhToan, // Hoặc tự sinh GUID
                MaDatTiec = model.MaDatTiec,
                NgayThanhToan = DateTime.Now,
                PhuongThucThanhToan = "Chuyển khoản/Tiền mặt", // Có thể lấy từ UI nếu cần
                TongTien = model.TongGiaTri, // Lưu tổng tiền chốt
                TrangThai = "Completed",
                GhiChu = "Thanh toán phần còn lại"
            };

            _context.PhieuThanhToans.Add(phieu);

            // Cập nhật trạng thái Đặt tiệc (nếu cần đồng bộ)
            datTiec.TrangThai = "Completed";

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Thanh toán thành công" });
        }
    }
}