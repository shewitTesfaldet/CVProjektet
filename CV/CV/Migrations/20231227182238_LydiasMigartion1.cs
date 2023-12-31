using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CV.Migrations
{
    /// <inheritdoc />
    public partial class LydiasMigartion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 1,
                column: "UID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 2,
                column: "UID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 3,
                column: "UID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 4,
                column: "UID",
                value: 4);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 5,
                column: "UID",
                value: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 1,
                column: "UID",
                value: null);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 2,
                column: "UID",
                value: null);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 3,
                column: "UID",
                value: null);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 4,
                column: "UID",
                value: null);

            migrationBuilder.UpdateData(
                table: "CV_s",
                keyColumn: "CID",
                keyValue: 5,
                column: "UID",
                value: null);
        }
    }
}
