using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class LienKetTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NHAN_VIEN_BO_PHAN_MaBoPhan",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "ChucVuNV",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "MailNV",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "MatKhau",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "TaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "EmailKhachHang",
                table: "KHACH_HANG");

            migrationBuilder.DropColumn(
                name: "MatKhauKhachHang",
                table: "KHACH_HANG");

            migrationBuilder.DropColumn(
                name: "TaiKhoanKhachHang",
                table: "KHACH_HANG");

            migrationBuilder.AlterColumn<string>(
                name: "MaBoPhan",
                table: "NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "MaTaiKhoan",
                table: "NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaTaiKhoan",
                table: "KHACH_HANG",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TAI_KHOAN",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAI_KHOAN", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NHAN_VIEN_MaTaiKhoan",
                table: "NHAN_VIEN",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_KHACH_HANG_MaTaiKhoan",
                table: "KHACH_HANG",
                column: "MaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_KHACH_HANG_TAI_KHOAN_MaTaiKhoan",
                table: "KHACH_HANG",
                column: "MaTaiKhoan",
                principalTable: "TAI_KHOAN",
                principalColumn: "MaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_NHAN_VIEN_BO_PHAN_MaBoPhan",
                table: "NHAN_VIEN",
                column: "MaBoPhan",
                principalTable: "BO_PHAN",
                principalColumn: "MaBoPhan");

            migrationBuilder.AddForeignKey(
                name: "FK_NHAN_VIEN_TAI_KHOAN_MaTaiKhoan",
                table: "NHAN_VIEN",
                column: "MaTaiKhoan",
                principalTable: "TAI_KHOAN",
                principalColumn: "MaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KHACH_HANG_TAI_KHOAN_MaTaiKhoan",
                table: "KHACH_HANG");

            migrationBuilder.DropForeignKey(
                name: "FK_NHAN_VIEN_BO_PHAN_MaBoPhan",
                table: "NHAN_VIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_NHAN_VIEN_TAI_KHOAN_MaTaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropTable(
                name: "TAI_KHOAN");

            migrationBuilder.DropIndex(
                name: "IX_NHAN_VIEN_MaTaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropIndex(
                name: "IX_KHACH_HANG_MaTaiKhoan",
                table: "KHACH_HANG");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "MaTaiKhoan",
                table: "KHACH_HANG");

            migrationBuilder.AlterColumn<string>(
                name: "MaBoPhan",
                table: "NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChucVuNV",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MailNV",
                table: "NHAN_VIEN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatKhau",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaiKhoan",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatKhauKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaiKhoanKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NHAN_VIEN_BO_PHAN_MaBoPhan",
                table: "NHAN_VIEN",
                column: "MaBoPhan",
                principalTable: "BO_PHAN",
                principalColumn: "MaBoPhan",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
