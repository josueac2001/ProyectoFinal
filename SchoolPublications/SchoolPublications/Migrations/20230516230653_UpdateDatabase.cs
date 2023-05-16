using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolPublications.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Categories_CategoryId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_CategoryId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_Id_CategoryId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "AdmissionDate",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "PublicationCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicationCategories_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicationImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationImages_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publications_Title",
                table: "Publications",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublicationCategories_CategoryId",
                table: "PublicationCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicationCategories_PublicationId_CategoryId",
                table: "PublicationCategories",
                columns: new[] { "PublicationId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublicationImages_PublicationId",
                table: "PublicationImages",
                column: "PublicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicationCategories");

            migrationBuilder.DropTable(
                name: "PublicationImages");

            migrationBuilder.DropIndex(
                name: "IX_Publications_Title",
                table: "Publications");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "AdmissionDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publications_CategoryId",
                table: "Publications",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_Id_CategoryId",
                table: "Publications",
                columns: new[] { "Id", "CategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Categories_CategoryId",
                table: "Publications",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
