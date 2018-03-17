using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog.WebApi.Migrations
{
    public partial class PostCategoryChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategorys_PostCategorys_PostCategoryId",
                table: "PostCategorys");

            migrationBuilder.DropColumn(
                name: "ParentPostCagegory",
                table: "PostCategorys");

            migrationBuilder.RenameColumn(
                name: "PostCategoryId",
                table: "PostCategorys",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategorys_PostCategoryId",
                table: "PostCategorys",
                newName: "IX_PostCategorys_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategorys_PostCategorys_ParentId",
                table: "PostCategorys",
                column: "ParentId",
                principalTable: "PostCategorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategorys_PostCategorys_ParentId",
                table: "PostCategorys");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "PostCategorys",
                newName: "PostCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategorys_ParentId",
                table: "PostCategorys",
                newName: "IX_PostCategorys_PostCategoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentPostCagegory",
                table: "PostCategorys",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategorys_PostCategorys_PostCategoryId",
                table: "PostCategorys",
                column: "PostCategoryId",
                principalTable: "PostCategorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
