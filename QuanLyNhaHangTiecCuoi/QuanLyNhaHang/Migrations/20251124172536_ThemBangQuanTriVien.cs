using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class ThemBangQuanTriVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QUAN_TRI_VIEN",
                columns: table => new
                {
                    MaQuanTriVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaTaiKhoan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenQuanTriVien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cccd = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SdtNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUAN_TRI_VIEN", x => x.MaQuanTriVien);
                    table.ForeignKey(
                        name: "FK_QUAN_TRI_VIEN_TAI_KHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "MaTaiKhoan");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QUAN_TRI_VIEN_MaTaiKhoan",
                table: "QUAN_TRI_VIEN",
                column: "MaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QUAN_TRI_VIEN");
        }
    }
}
