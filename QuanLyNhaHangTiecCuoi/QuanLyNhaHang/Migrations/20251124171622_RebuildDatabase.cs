using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class RebuildDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BO_PHAN",
                columns: table => new
                {
                    MaBoPhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenBoPhan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TienCong = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BO_PHAN", x => x.MaBoPhan);
                });

            migrationBuilder.CreateTable(
                name: "COMBO_MON",
                columns: table => new
                {
                    MaComboMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenCombo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayTaoCombo = table.Column<DateTime>(type: "date", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    GiaCombo = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhAnhCombo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMBO_MON", x => x.MaComboMon);
                });

            migrationBuilder.CreateTable(
                name: "DICH_VU",
                columns: table => new
                {
                    MaDichVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GiaDV = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    MoTaDV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiDV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhDichVu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DICH_VU", x => x.MaDichVu);
                });

            migrationBuilder.CreateTable(
                name: "MON_AN",
                columns: table => new
                {
                    MaMonAn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenMonAn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MoTaMonAn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LoaiMonAn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThaiMonAn = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhMonAn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MON_AN", x => x.MaMonAn);
                });

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

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Token);
                });

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

            migrationBuilder.CreateTable(
                name: "NHAN_VIEN",
                columns: table => new
                {
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaBoPhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenNhanVien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CccdNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SdtNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DiaChiNV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MailNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChucVuNV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TrangThaiNV = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHAN_VIEN", x => x.MaNhanVien);
                    table.ForeignKey(
                        name: "FK_NHAN_VIEN_BO_PHAN_MaBoPhan",
                        column: x => x.MaBoPhan,
                        principalTable: "BO_PHAN",
                        principalColumn: "MaBoPhan");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_COMBO",
                columns: table => new
                {
                    MaChiTietCombo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    MaComboMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaMonAn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_COMBO", x => x.MaChiTietCombo);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_COMBO_COMBO_MON_MaComboMon",
                        column: x => x.MaComboMon,
                        principalTable: "COMBO_MON",
                        principalColumn: "MaComboMon");
                    table.ForeignKey(
                        name: "FK_CHI_TIET_COMBO_MON_AN_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MON_AN",
                        principalColumn: "MaMonAn");
                });

            migrationBuilder.CreateTable(
                name: "HINH_ANH",
                columns: table => new
                {
                    MaHinhAnh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaMonAn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaDichVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiHinhAnh = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HINH_ANH", x => x.MaHinhAnh);
                    table.ForeignKey(
                        name: "FK_HINH_ANH_DICH_VU_MaDichVu",
                        column: x => x.MaDichVu,
                        principalTable: "DICH_VU",
                        principalColumn: "MaDichVu");
                    table.ForeignKey(
                        name: "FK_HINH_ANH_MON_AN_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MON_AN",
                        principalColumn: "MaMonAn");
                });

            migrationBuilder.CreateTable(
                name: "KHACH_HANG",
                columns: table => new
                {
                    MaKhachHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaTaiKhoan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CccdKhachHang = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SdtKhachHang = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiaChiKhachHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiKhachHang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHACH_HANG", x => x.MaKhachHang);
                    table.ForeignKey(
                        name: "FK_KHACH_HANG_TAI_KHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAI_KHOAN",
                        principalColumn: "MaTaiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "DAT_TIEC",
                columns: table => new
                {
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenCoDau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TenChuRe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayDatTiec = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayToChuc = table.Column<DateTime>(type: "date", nullable: true),
                    GioToChuc = table.Column<TimeSpan>(type: "time", nullable: true),
                    SoBanDat = table.Column<int>(type: "int", nullable: true),
                    GiaBan = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TienCoc = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAT_TIEC", x => x.MaDatTiec);
                    table.ForeignKey(
                        name: "FK_DAT_TIEC_KHACH_HANG_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KHACH_HANG",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "CHI_TIET_THUC_DON",
                columns: table => new
                {
                    MaChiTietThucDon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaMonAn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoLuongMotBan = table.Column<int>(type: "int", nullable: true),
                    GhiChuThem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHI_TIET_THUC_DON", x => x.MaChiTietThucDon);
                    table.ForeignKey(
                        name: "FK_CHI_TIET_THUC_DON_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec");
                    table.ForeignKey(
                        name: "FK_CHI_TIET_THUC_DON_MON_AN_MaMonAn",
                        column: x => x.MaMonAn,
                        principalTable: "MON_AN",
                        principalColumn: "MaMonAn");
                });

            migrationBuilder.CreateTable(
                name: "PHIEU_THANH_TOAN",
                columns: table => new
                {
                    MaPhieu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaDichVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    NgaySuDung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TT_SU_DUNG_DICH_VU", x => x.MaThongTinDV);
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_DICH_VU_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec");
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_DICH_VU_DICH_VU_MaDichVu",
                        column: x => x.MaDichVu,
                        principalTable: "DICH_VU",
                        principalColumn: "MaDichVu");
                });

            migrationBuilder.CreateTable(
                name: "TT_SU_DUNG_NHAN_VIEN",
                columns: table => new
                {
                    MaThongTinNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaDatTiec = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaNhanVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaNhanVienPT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TT_SU_DUNG_NHAN_VIEN", x => x.MaThongTinNV);
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_NHAN_VIEN_DAT_TIEC_MaDatTiec",
                        column: x => x.MaDatTiec,
                        principalTable: "DAT_TIEC",
                        principalColumn: "MaDatTiec");
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NHAN_VIEN",
                        principalColumn: "MaNhanVien");
                    table.ForeignKey(
                        name: "FK_TT_SU_DUNG_NHAN_VIEN_NHAN_VIEN_PART_TIME_MaNhanVienPT",
                        column: x => x.MaNhanVienPT,
                        principalTable: "NHAN_VIEN_PART_TIME",
                        principalColumn: "MaNhanVienPT");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_COMBO_MaComboMon",
                table: "CHI_TIET_COMBO",
                column: "MaComboMon");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_COMBO_MaMonAn",
                table: "CHI_TIET_COMBO",
                column: "MaMonAn");

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
                name: "IX_HINH_ANH_MaDichVu",
                table: "HINH_ANH",
                column: "MaDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_HINH_ANH_MaMonAn",
                table: "HINH_ANH",
                column: "MaMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_KHACH_HANG_MaTaiKhoan",
                table: "KHACH_HANG",
                column: "MaTaiKhoan");

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

            migrationBuilder.CreateIndex(
                name: "IX_TT_SU_DUNG_NHAN_VIEN_MaNhanVienPT",
                table: "TT_SU_DUNG_NHAN_VIEN",
                column: "MaNhanVienPT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHI_TIET_COMBO");

            migrationBuilder.DropTable(
                name: "CHI_TIET_THUC_DON");

            migrationBuilder.DropTable(
                name: "HINH_ANH");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "PHIEU_THANH_TOAN");

            migrationBuilder.DropTable(
                name: "TT_SU_DUNG_DICH_VU");

            migrationBuilder.DropTable(
                name: "TT_SU_DUNG_NHAN_VIEN");

            migrationBuilder.DropTable(
                name: "COMBO_MON");

            migrationBuilder.DropTable(
                name: "MON_AN");

            migrationBuilder.DropTable(
                name: "DICH_VU");

            migrationBuilder.DropTable(
                name: "DAT_TIEC");

            migrationBuilder.DropTable(
                name: "NHAN_VIEN");

            migrationBuilder.DropTable(
                name: "NHAN_VIEN_PART_TIME");

            migrationBuilder.DropTable(
                name: "KHACH_HANG");

            migrationBuilder.DropTable(
                name: "BO_PHAN");

            migrationBuilder.DropTable(
                name: "TAI_KHOAN");
        }
    }
}
