using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.db
{
    public class QuanLyNhaHangContextFactory : IDesignTimeDbContextFactory<QuanLyNhaHangContext>
    {
        public QuanLyNhaHangContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuanLyNhaHangContext>();
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-UKUNQ3QE\SQLEXPRESS;Initial Catalog=QL_NhaHangTiecCuoiLongPhung;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

            return new QuanLyNhaHangContext(optionsBuilder.Options);
        }
    }
}