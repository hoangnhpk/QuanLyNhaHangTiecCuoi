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
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-ANJJC90;Initial Catalog=QL_NhaHangTiecCuoiLongPhung;Integrated Security=True;Trust Server Certificate=True");

            return new QuanLyNhaHangContext(optionsBuilder.Options);
        }
    }
}