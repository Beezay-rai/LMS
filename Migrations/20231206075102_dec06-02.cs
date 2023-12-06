using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Migrations
{
    public partial class dec0602 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Book_BookId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Student_StudentId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BookId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_StudentId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "IssuedDate",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Transaction");

            migrationBuilder.CreateTable(
                name: "BookTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookTransaction_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTransaction_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTransaction_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookTransaction_BookId",
                table: "BookTransaction",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTransaction_StudentId",
                table: "BookTransaction",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTransaction_TransactionId",
                table: "BookTransaction",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookTransaction");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedDate",
                table: "Transaction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Transaction",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookId",
                table: "Transaction",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StudentId",
                table: "Transaction",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Book_BookId",
                table: "Transaction",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Student_StudentId",
                table: "Transaction",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
