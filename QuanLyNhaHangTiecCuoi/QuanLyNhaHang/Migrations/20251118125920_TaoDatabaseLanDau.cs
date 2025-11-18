using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class TaoDatabaseLanDau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BO_PHAN",
                columns: table => new
                {
                    MaBoPhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenBoPhan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TienCong = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BO_PHAN", x => x.MaBoPhan);
                });

            migrationBuilder.CreateTable(
                name: "DICH_VU",
                columns: table => new
                {
                    MaDichVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaDV = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    MoTaDV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiDV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DICH_VU", x => x.MaDichVu);
                });

            migrationBuilder.CreateTable(
                name: "HINH_ANH",
                columns: table => new
                {
                    MaHinhAnh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaChuThe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiHinhAnh = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HINH_ANH", x => x.MaHinhAnh);
                });

            migrationBuilder.CreateTable(
                name: "KHACH_HANG",
                columns: table => new
                {
                    MaKhachHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CccdKhachHang = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SdtKhachHang = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChiKhachHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaiKhoanKhachHang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MatKhauKhachHang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TrangThaiKhachHang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHACH_HANG", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "MON_AN",
                columns: table => new
                {
                    MaMonAn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenMonAn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    MoTaMonAn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LoaiMonAn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThaiMonAn = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MON_AN", x => x.MaMonAn);
                });

            migrationBuilder.CreateTable(
                name: "NHAN_VIEN",
                columns: table => new
                {
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaBoPhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CccdNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SdtNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChiNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MailNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaiKhoan = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TrangThaiNV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHAN_VIEN", x => x.MaNhanVien);
                    table.ForeignKey(
                        name: "FK_NHAN_VIEN_BO_PHAN_MaBoPhan",
                        column: x => x.MaBoPhan,
                        principalTable: "BO_PHAN",
                        principalColumn: "MaBoPhan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAT_TIEC",
                columns: table => new
                {
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenCoDau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TenChuRe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayDatTiec = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayToChuc = table.Column<DateTime>(type: "date", nullable: true),
                    GioToChuc = table.Column<TimeSpan>(type: "time", nullable: true),
                    SoBanDat = table.Column<int>(type: "int", nullable: true),
                    GiaBan = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TienCoc = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAT_TIEC", x => x.MaDatTiec);
                    table.ForeignKey(
                        name: "FK_DAT_TIEC_KHACH_HANG_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KHACH_HANG",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_THUC_DON",
                columns: table => new
                {
                    MaChiTietThucDon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaMonAn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoLuongMotBan = table.Column<int>(type: "int", nullable: true),
                    GhiChuThem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_THUC_DON", x => x.MaChiTietThucDon);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_THUC_DON_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_THUC_DON_MON_AN_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MON_AN",
                        principalColumn: "MaMonAn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHIEU_THANH_TOAN",
                columns: table => new
                {
                    MaPhieu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEU_THANH_TOAN", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PHIEU_THANH_TOAN_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TT_SU_DUNG_DICH_VU",
                columns: table => new
                {
                    MaThongTinDV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDichVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    NgaySuDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TT_SU_DUNG_DICH_VU", x => x.MaThongTinDV);
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_DICH_VU_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_DICH_VU_DICH_VU_MaDichVu",
                        column: x => x.MaDichVu,
                        principalTable: "DICH_VU",
                        principalColumn: "MaDichVu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TT_SU_DUNG_NHAN_VIEN",
                columns: table => new
                {
                    MaThongTinNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TT_SU_DUNG_NHAN_VIEN", x => x.MaThongTinNV);
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_NHAN_VIEN_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NHAN_VIEN",
                        principalColumn: "MaNhanVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_THUC_DON_MaDatTiec",
                table: "CHI_TIET_THUC_DON",
                column: "MaDatTiec");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_THUC_DON_MaMonAn",
                table: "CHI_TIET_THUC_DON",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_DAT_TIEC_MaKhachHang",
                table: "DAT_TIEC",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_NHAN_VIEN_MaBoPhan",
                table: "NHAN_VIEN",
                column: "MaBoPhan");

            migrationBuilder.CreateIndex(
                name: "IX_PHIEU_THANH_TOAN_MaDatTiec",
                table: "PHIEU_THANH_TOAN",
                column: "MaDatTiec",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TT_SU_DUNG_DICH_VU_MaDatTiec",
                table: "TT_SU_DUNG_DICH_VU",
                column: "MaDatTiec");

            migrationBuilder.CreateIndex(
                name: "IX_TT_SU_DUNG_DICH_VU_MaDichVu",
                table: "TT_SU_DUNG_DICH_VU",
                column: "MaDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_TT_SU_DUNG_NHAN_VIEN_MaDatTiec",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaDatTiec");

            migrationBuilder.CreateIndex(
                name: "IX_TT_SU_DUNG_NHAN_VIEN_MaNhanVien",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaNhanVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHI_TIET_THUC_DON");

            migrationBuilder.DropTable(
                name: "HINH_ANH");

            migrationBuilder.DropTable(
                name: "PHIEU_THANH_TOAN");

            migrationBuilder.DropTable(
                name: "TT_SU_DUNG_DICH_VU");

            migrationBuilder.DropTable(
                name: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.DropTable(
                name: "MON_AN");

            migrationBuilder.DropTable(
                name: "DICH_VU");

            migrationBuilder.DropTable(
                name: "DAT_TIEC");

            migrationBuilder.DropTable(
                name: "NHAN_VIEN");

            migrationBuilder.DropTable(
                name: "KHACH_HANG");

            migrationBuilder.DropTable(
                name: "BO_PHAN");
        }
    }
}
