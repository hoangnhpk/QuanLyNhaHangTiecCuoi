namespace QuanLyNhaHang.Services
{
    public interface IEmailService
    {
        // Hàm gửi email cơ bản
        void GuiEmail(string denEmail, string tieuDe, string noiDung);
        
    }
}