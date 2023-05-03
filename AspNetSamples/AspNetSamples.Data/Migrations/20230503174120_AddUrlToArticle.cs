using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetSamples.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArticleSourceUrl",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleSourceUrl",
                table: "Articles");
        }
    }
}
