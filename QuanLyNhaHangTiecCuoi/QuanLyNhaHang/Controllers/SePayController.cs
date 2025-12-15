using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using System.Text.RegularExpressions;

namespace QuanLyNhaHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SePayController : ControllerBase
    {
        private readonly QuanLyNhaHangContext _context;
        // Dán API Key SePay của bạn vào đây
        private const string SEPAY_API_KEY = "UK19...";

        public SePayController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        [HttpPost("Webhook")]
        public IActionResult ReceivePayment([FromBody] SePayWebhookData data)
        {
            try
            {
                // Log ra cửa sổ Output của Visual Studio để kiểm tra
                Console.WriteLine($"[SePay] Nhận tiền: {data.transferAmount} - Nội dung: {data.content}");

                string noiDungCK = data.content.ToUpper();
                // Tìm mã đơn hàng DTxxx
                var match = Regex.Match(noiDungCK, @"DT\d+");

                if (match.Success)
                {
                    string maDatTiec = match.Value;
                    var tiec = _context.DatTiecs.Find(maDatTiec);

                    if (tiec != null)
                    {
                        // Kiểm tra tiền (Cho phép sai số nhỏ hoặc chuyển dư)
                        if (data.transferAmount >= (tiec.TienCoc ?? 0))
                        {
                            tiec.TrangThai = "Đã cọc 30%";
                            _context.SaveChanges();
                            return Ok(new { success = true, message = "Cập nhật thành công" });
                        }
                    }
                }
                return Ok(new { success = false, message = "Không khớp đơn hàng" });
            }
            catch (Exception ex)
            {
                // In lỗi ra để biết đường sửa
                Console.WriteLine("Lỗi Webhook: " + ex.Message);
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    // Class dữ liệu chuẩn của SePay
    public class SePayWebhookData
    {
        public long id { get; set; }
        public string gateway { get; set; }
        public string transactionDate { get; set; }
        public string accountNumber { get; set; }
        public string content { get; set; }
        public decimal transferAmount { get; set; }
        public string transferType { get; set; } // <-- ĐÃ SỬA: Phải là string (vì SePay trả về "in" hoặc "out")
    }
}