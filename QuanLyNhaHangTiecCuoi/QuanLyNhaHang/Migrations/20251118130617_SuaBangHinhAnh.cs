using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class SuaBangHinhAnh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaChuThe",
                table: "HINH_ANH",
                newName: "MaMonAn");

            migrationBuilder.AddColumn<string>(
                name: "MaDichVu",
                table: "HINH_ANH",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HINH_ANH_MaDichVu",
                table: "HINH_ANH",
                column: "MaDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_HINH_ANH_MaMonAn",
                table: "HINH_ANH",
                column: "MaMonAn");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HINH_ANH_DICH_VU_MaDichVu",
                table: "HINH_ANH");

            migrationBuilder.DropForeignKey(
                name: "FK_HINH_ANH_MON_AN_MaMonAn",
                table: "HINH_ANH");

            migrationBuilder.DropIndex(
                name: "IX_HINH_ANH_MaDichVu",
                table: "HINH_ANH");

            migrationBuilder.DropIndex(
                name: "IX_HINH_ANH_MaMonAn",
                table: "HINH_ANH");

            migrationBuilder.DropColumn(
                name: "MaDichVu",
                table: "HINH_ANH");

            migrationBuilder.RenameColumn(
                name: "MaMonAn",
                table: "HINH_ANH",
                newName: "MaChuThe");
        }
    }
}
