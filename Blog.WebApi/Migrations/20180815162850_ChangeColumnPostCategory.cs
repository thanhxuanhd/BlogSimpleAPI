using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.WebApi.Migrations
{
    public partial class ChangeColumnPostCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CagegoryDescription",
                table: "PostCategorys",
                newName: "CategoryDescription");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                table: "AutoHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2048,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryDescription",
                table: "PostCategorys",
                newName: "CagegoryDescription");

            migrationBuilder.AlterColumn<string>(
                name: "Changed",
                table: "AutoHistory",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}