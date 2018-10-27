using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.WebApi.Migrations
{
    public partial class AddRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId1",
                table: "AppUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId1",
                table: "AppUserTokens");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId1",
                table: "AppUserTokens",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenHash",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId1",
                table: "AppUserLogins",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId1",
                table: "AppUserLogins",
                column: "UserId1",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId1",
                table: "AppUserTokens",
                column: "UserId1",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId1",
                table: "AppUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId1",
                table: "AppUserTokens");

            migrationBuilder.DropColumn(
                name: "RefreshTokenHash",
                table: "AppUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId1",
                table: "AppUserTokens",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId1",
                table: "AppUserLogins",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId1",
                table: "AppUserLogins",
                column: "UserId1",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId1",
                table: "AppUserTokens",
                column: "UserId1",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
