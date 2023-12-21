using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBook.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Publisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublisherId1",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId2",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId1",
                table: "Books",
                column: "PublisherId1");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId2",
                table: "Books",
                column: "PublisherId2",
                unique: true,
                filter: "[PublisherId2] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId1",
                table: "Books",
                column: "PublisherId1",
                principalTable: "Publishers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId2",
                table: "Books",
                column: "PublisherId2",
                principalTable: "Publishers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId1",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId2",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherId1",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherId2",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublisherId1",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublisherId2",
                table: "Books");
        }
    }
}
