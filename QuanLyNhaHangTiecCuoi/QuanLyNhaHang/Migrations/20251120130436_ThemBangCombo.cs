using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class ThemBangCombo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChucVuNV",
                table: "NHAN_VIEN",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhMonAn",
                table: "MON_AN",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhDichVu",
                table: "DICH_VU",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "COMBO_MON",
                columns: table => new
                {
                    MaComboMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenCombo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayTaoCombo = table.Column<DateTime>(type: "date", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    GiaCombo = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhCombo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMBO_MON", x => x.MaComboMon);
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

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_COMBO_MaComboMon",
                table: "CHI_TIET_COMBO",
                column: "MaComboMon");

            migrationBuilder.CreateIndex(
                name: "IX_CHI_TIET_COMBO_MaMonAn",
                table: "CHI_TIET_COMBO",
                column: "MaMonAn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHI_TIET_COMBO");

            migrationBuilder.DropTable(
                name: "COMBO_MON");

            migrationBuilder.DropColumn(
                name: "ChucVuNV",
                table: "NHAN_VIEN");

            migrationBuilder.DropColumn(
                name: "HinhAnhMonAn",
                table: "MON_AN");

            migrationBuilder.DropColumn(
                name: "HinhAnhDichVu",
                table: "DICH_VU");
        }
    }
}
