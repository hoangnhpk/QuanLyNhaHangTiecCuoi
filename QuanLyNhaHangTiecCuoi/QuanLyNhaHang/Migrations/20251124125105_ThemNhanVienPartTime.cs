using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class ThemNhanVienPartTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NHAN_VIEN_TAI_KHOAN_MaTaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropIndex(
                name: "IX_NHAN_VIEN_MaTaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "MatKhau",
                table: "NHAN_VIEN");

            migrationBuilder.AddColumn<string>(
                name: "MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenCombo",
                table: "COMBO_MON",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "COMBO_MON",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GiaCombo",
                table: "COMBO_MON",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NHAN_VIEN_PART_TIME",
                columns: table => new
                {
                    MaNhanVienPT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenNhanVienPT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CccdNVPT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SdtNVPT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DiaChiNVPT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmailNVPT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThaiNV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHAN_VIEN_PART_TIME", x => x.MaNhanVienPT);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TT_SU_DUNG_NHAN_VIEN_MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaNhanVienPT");

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_PART_TIME_MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaNhanVienPT",
                principalTable: "NHAN_VIEN_PART_TIME",
                principalColumn: "MaNhanVienPT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_PART_TIME_MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.DropTable(
                name: "NHAN_VIEN_PART_TIME");

            migrationBuilder.DropIndex(
                name: "IX_TT_SU_DUNG_NHAN_VIEN_MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.AddColumn<string>(
                name: "MaTaiKhoan",
                table: "NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatKhau",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenCombo",
                table: "COMBO_MON",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "COMBO_MON",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GiaCombo",
                table: "COMBO_MON",
                type: "decimal(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.CreateIndex(
                name: "IX_NHAN_VIEN_MaTaiKhoan",
                table: "NHAN_VIEN",
                column: "MaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_NHAN_VIEN_TAI_KHOAN_MaTaiKhoan",
                table: "NHAN_VIEN",
                column: "MaTaiKhoan",
                principalTable: "TAI_KHOAN",
                principalColumn: "MaTaiKhoan");
        }
    }
}
