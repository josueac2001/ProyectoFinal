using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolPublications.Migrations
{
    /// <inheritdoc />
    public partial class updateColumnsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Publications");

            migrationBuilder.RenameColumn(
                name: "Tittle",
                table: "Publications",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Remark",
                table: "Comments",
                newName: "Content");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "CommentDate",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Publications",
                newName: "Tittle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comments",
                newName: "Remark");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Publications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Publications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
