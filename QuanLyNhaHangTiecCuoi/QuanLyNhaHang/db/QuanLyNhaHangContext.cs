using Microsoft.EntityFrameworkCore; // Thư viện quan trọng nhất
using QuanLyNhaHang.Models; // Để nhận diện được các class bạn vừa tạo

namespace QuanLyNhaHang.Models
{
    public class QuanLyNhaHangContext : DbContext
    {
        // Constructor này giúp nhận chuỗi kết nối từ Program.cs
        public QuanLyNhaHangContext(DbContextOptions<QuanLyNhaHangContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Sửa lại cho khớp với máy tính của bạn (SQLEXPRESS)
                // Lưu ý: Trong C# string dùng @ đằng trước thì chỉ cần 1 dấu \
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-ANJJC90;Initial Catalog=QL_NhaHangTiecCuoiLongPhung;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        // Khai báo các bảng sẽ được tạo trong Database
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<BoPhan> BoPhans { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<DatTiec> DatTiecs { get; set; }
        public DbSet<PhieuThanhToan> PhieuThanhToans { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }

        // Các bảng trung gian
        public DbSet<ChiTietThucDon> ChiTietThucDons { get; set; }
        public DbSet<TT_SuDungDichVu> TT_SuDungDichVus { get; set; }
        public DbSet<TT_SuDungNhanVien> TT_SuDungNhanViens { get; set; }

        // Cấu hình thêm (nếu cần) để xử lý các quan hệ phức tạp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- CẤU HÌNH KIỂU DỮ LIỆU TIỀN TỆ (DECIMAL) ---
            // Định dạng decimal(18, 0) nghĩa là tối đa 18 chữ số, 0 số lẻ thập phân (Phù hợp VND)

            modelBuilder.Entity<BoPhan>()
                .Property(p => p.TienCong).HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<MonAn>()
                .Property(p => p.DonGia).HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<DichVu>()
                .Property(p => p.GiaDV).HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<DatTiec>()
                .Property(p => p.GiaBan).HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<DatTiec>()
                .Property(p => p.TienCoc).HasColumnType("decimal(18, 0)");

            modelBuilder.Entity<PhieuThanhToan>()
                .Property(p => p.TongTien).HasColumnType("decimal(18, 0)");

            // Ví dụ: Đảm bảo quan hệ 1-1 giữa DatTiec và PhieuThanhToan
            modelBuilder.Entity<DatTiec>()
                .HasOne(a => a.PhieuThanhToan)
                .WithOne(b => b.DatTiec)
                .HasForeignKey<PhieuThanhToan>(b => b.MaDatTiec);
        }
    }
}