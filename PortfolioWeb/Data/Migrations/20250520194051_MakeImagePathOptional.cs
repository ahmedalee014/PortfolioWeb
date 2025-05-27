using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeImagePathOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_AuthorId",
                table: "BlogPosts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_AuthorId",
                table: "BlogPosts");
        }
    }
}
