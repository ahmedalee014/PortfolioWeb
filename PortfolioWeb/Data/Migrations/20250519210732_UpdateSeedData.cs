using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "BlogPosts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_IsPublished",
                table: "BlogPosts",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PublishedDate",
                table: "BlogPosts",
                column: "PublishedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_IsPublished",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_PublishedDate",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
