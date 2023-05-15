using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolPublications.Migrations
{
    /// <inheritdoc />
    public partial class ApprovalDroped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aprovals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aprovals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Aprovals_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aprovals_Id_PublicationId",
                table: "Aprovals",
                columns: new[] { "Id", "PublicationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aprovals_PublicationId",
                table: "Aprovals",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Aprovals_UserId",
                table: "Aprovals",
                column: "UserId");
        }
    }
}
