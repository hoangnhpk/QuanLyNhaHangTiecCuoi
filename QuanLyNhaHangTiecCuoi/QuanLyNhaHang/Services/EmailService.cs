using System.Net;
using System.Net.Mail;

namespace QuanLyNhaHang.Services
{
    public class EmailService : IEmailService
    {
        public void GuiEmail(string denEmail, string tieuDe, string noiDung)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    // Lưu ý: Nên đưa email/pass vào appsettings.json thay vì hard-code thế này (để bảo mật)
                    Credentials = new NetworkCredential("aunxpk04299@gmail.com", "fqme nhhv ngbv rsed"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("aunxpk04299@gmail.com"),
                    Subject = tieuDe,
                    Body = noiDung,
                    IsBodyHtml = true, // Để gửi được mã HTML đẹp
                };

                mailMessage.To.Add(denEmail);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                Console.WriteLine("Lỗi gửi email: " + ex.Message);
                throw; // Ném lỗi ra để Controller biết mà xử lý
            }
        }
    }
}