using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChoPhepNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CHI_TIET_THUC_DON_DAT_TIEC_MaDatTiec",
                table: "CHI_TIET_THUC_DON");

            migrationBuilder.DropForeignKey(
                name: "FK_CHI_TIET_THUC_DON_MON_AN_MaMonAn",
                table: "CHI_TIET_THUC_DON");

            migrationBuilder.DropForeignKey(
                name: "FK_DAT_TIEC_KHACH_HANG_MaKhachHang",
                table: "DAT_TIEC");

            migrationBuilder.DropForeignKey(
                name: "FK_HINH_ANH_DICH_VU_MaDichVu",
                table: "HINH_ANH");

            migrationBuilder.DropForeignKey(
                name: "FK_HINH_ANH_MON_AN_MaMonAn",
                table: "HINH_ANH");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DICH_VU_MaDichVu",
                table: "TT_SU_DUNG_DICH_VU");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.AlterColumn<string>(
                name: "MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaDichVu",
                table: "TT_SU_DUNG_DICH_VU",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "TT_SU_DUNG_DICH_VU",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "PHIEU_THANH_TOAN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "PHIEU_THANH_TOAN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "PHIEU_THANH_TOAN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiNV",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhanVien",
                table: "NHAN_VIEN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TaiKhoan",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SdtNV",
                table: "NHAN_VIEN",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhau",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MailNV",
                table: "NHAN_VIEN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "NHAN_VIEN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiNV",
                table: "NHAN_VIEN",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CccdNV",
                table: "NHAN_VIEN",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiMonAn",
                table: "MON_AN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "TenMonAn",
                table: "MON_AN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaMonAn",
                table: "MON_AN",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiMonAn",
                table: "MON_AN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "MON_AN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DonViTinh",
                table: "MON_AN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "TenKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TaiKhoanKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SdtKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhauKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "KHACH_HANG",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmailKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CccdKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "HINH_ANH",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiHinhAnh",
                table: "HINH_ANH",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MaMonAn",
                table: "HINH_ANH",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "MaDichVu",
                table: "HINH_ANH",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "HINH_ANH",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiDV",
                table: "DICH_VU",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "TenDichVu",
                table: "DICH_VU",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaDV",
                table: "DICH_VU",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "DICH_VU",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "DAT_TIEC",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "TenCoDau",
                table: "DAT_TIEC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TenChuRe",
                table: "DAT_TIEC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MaKhachHang",
                table: "DAT_TIEC",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "ChiTiet",
                table: "DAT_TIEC",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaMonAn",
                table: "CHI_TIET_THUC_DON",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatTiec",
                table: "CHI_TIET_THUC_DON",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChuThem",
                table: "CHI_TIET_THUC_DON",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "BO_PHAN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "TenBoPhan",
                table: "BO_PHAN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "BO_PHAN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_CHI_TIET_THUC_DON_DAT_TIEC_MaDatTiec",
                table: "CHI_TIET_THUC_DON",
                column: "MaDatTiec",
                principalTable: "DAT_TIEC",
                principalColumn: "MaDatTiec");

            migrationBuilder.AddForeignKey(
                name: "FK_CHI_TIET_THUC_DON_MON_AN_MaMonAn",
                table: "CHI_TIET_THUC_DON",
                column: "MaMonAn",
                principalTable: "MON_AN",
                principalColumn: "MaMonAn");

            migrationBuilder.AddForeignKey(
                name: "FK_DAT_TIEC_KHACH_HANG_MaKhachHang",
                table: "DAT_TIEC",
                column: "MaKhachHang",
                principalTable: "KHACH_HANG",
                principalColumn: "MaKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HINH_ANH_DICH_VU_MaDichVu",
                table: "HINH_ANH",
                column: "MaDichVu",
                principalTable: "DICH_VU",
                principalColumn: "MaDichVu");

            migrationBuilder.AddForeignKey(
                name: "FK_HINH_ANH_MON_AN_MaMonAn",
                table: "HINH_ANH",
                column: "MaMonAn",
                principalTable: "MON_AN",
                principalColumn: "MaMonAn");

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU",
                column: "MaDatTiec",
                principalTable: "DAT_TIEC",
                principalColumn: "MaDatTiec");

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DICH_VU_MaDichVu",
                table: "TT_SU_DUNG_DICH_VU",
                column: "MaDichVu",
                principalTable: "DICH_VU",
                principalColumn: "MaDichVu");

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaDatTiec",
                principalTable: "DAT_TIEC",
                principalColumn: "MaDatTiec");

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaNhanVien",
                principalTable: "NHAN_VIEN",
                principalColumn: "MaNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CHI_TIET_THUC_DON_DAT_TIEC_MaDatTiec",
                table: "CHI_TIET_THUC_DON");

            migrationBuilder.DropForeignKey(
                name: "FK_CHI_TIET_THUC_DON_MON_AN_MaMonAn",
                table: "CHI_TIET_THUC_DON");

            migrationBuilder.DropForeignKey(
                name: "FK_DAT_TIEC_KHACH_HANG_MaKhachHang",
                table: "DAT_TIEC");

            migrationBuilder.DropForeignKey(
                name: "FK_HINH_ANH_DICH_VU_MaDichVu",
                table: "HINH_ANH");

            migrationBuilder.DropForeignKey(
                name: "FK_HINH_ANH_MON_AN_MaMonAn",
                table: "HINH_ANH");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DICH_VU_MaDichVu",
                table: "TT_SU_DUNG_DICH_VU");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.AlterColumn<string>(
                name: "MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "TT_SU_DUNG_NHAN_VIEN",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaDichVu",
                table: "TT_SU_DUNG_DICH_VU",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "TT_SU_DUNG_DICH_VU",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "PHIEU_THANH_TOAN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "PHIEU_THANH_TOAN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "PHIEU_THANH_TOAN",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiNV",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenNhanVien",
                table: "NHAN_VIEN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaiKhoan",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SdtNV",
                table: "NHAN_VIEN",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhau",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MailNV",
                table: "NHAN_VIEN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "NHAN_VIEN",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiNV",
                table: "NHAN_VIEN",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CccdNV",
                table: "NHAN_VIEN",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiMonAn",
                table: "MON_AN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenMonAn",
                table: "MON_AN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaMonAn",
                table: "MON_AN",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiMonAn",
                table: "MON_AN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "MON_AN",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonViTinh",
                table: "MON_AN",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaiKhoanKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SdtKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MatKhauKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "KHACH_HANG",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CccdKhachHang",
                table: "KHACH_HANG",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "HINH_ANH",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiHinhAnh",
                table: "HINH_ANH",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaMonAn",
                table: "HINH_ANH",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaDichVu",
                table: "HINH_ANH",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "HINH_ANH",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThaiDV",
                table: "DICH_VU",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenDichVu",
                table: "DICH_VU",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoTaDV",
                table: "DICH_VU",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "DICH_VU",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "DAT_TIEC",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenCoDau",
                table: "DAT_TIEC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenChuRe",
                table: "DAT_TIEC",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaKhachHang",
                table: "DAT_TIEC",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChiTiet",
                table: "DAT_TIEC",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaMonAn",
                table: "CHI_TIET_THUC_DON",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatTiec",
                table: "CHI_TIET_THUC_DON",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChuThem",
                table: "CHI_TIET_THUC_DON",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "BO_PHAN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenBoPhan",
                table: "BO_PHAN",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                table: "BO_PHAN",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CHI_TIET_THUC_DON_DAT_TIEC_MaDatTiec",
                table: "CHI_TIET_THUC_DON",
                column: "MaDatTiec",
                principalTable: "DAT_TIEC",
                principalColumn: "MaDatTiec",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CHI_TIET_THUC_DON_MON_AN_MaMonAn",
                table: "CHI_TIET_THUC_DON",
                column: "MaMonAn",
                principalTable: "MON_AN",
                principalColumn: "MaMonAn",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DAT_TIEC_KHACH_HANG_MaKhachHang",
                table: "DAT_TIEC",
                column: "MaKhachHang",
                principalTable: "KHACH_HANG",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HINH_ANH_DICH_VU_MaDichVu",
                table: "HINH_ANH",
                column: "MaDichVu",
                principalTable: "DICH_VU",
                principalColumn: "MaDichVu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HINH_ANH_MON_AN_MaMonAn",
                table: "HINH_ANH",
                column: "MaMonAn",
                principalTable: "MON_AN",
                principalColumn: "MaMonAn",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU",
                column: "MaDatTiec",
                principalTable: "DAT_TIEC",
                principalColumn: "MaDatTiec",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_DICH_VU_DICH_VU_MaDichVu",
                table: "TT_SU_DUNG_DICH_VU",
                column: "MaDichVu",
                principalTable: "DICH_VU",
                principalColumn: "MaDichVu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_DAT_TIEC_MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaDatTiec",
                principalTable: "DAT_TIEC",
                principalColumn: "MaDatTiec",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaNhanVien",
                principalTable: "NHAN_VIEN",
                principalColumn: "MaNhanVien",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
