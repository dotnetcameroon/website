using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerTitleAndSubtitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubtitleEn",
                table: "Banners",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubtitleFr",
                table: "Banners",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Banners",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFr",
                table: "Banners",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubtitleEn",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "SubtitleFr",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "TitleFr",
                table: "Banners");
        }
    }
}
