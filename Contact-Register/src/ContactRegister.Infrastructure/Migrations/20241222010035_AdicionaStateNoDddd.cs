using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactRegister.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaStateNoDddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                table: "tb_ddd",
                newName: "region");

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "tb_ddd",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ddd_code",
                table: "tb_ddd",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_ddd_code",
                table: "tb_ddd");

            migrationBuilder.DropColumn(
                name: "state",
                table: "tb_ddd");

            migrationBuilder.RenameColumn(
                name: "region",
                table: "tb_ddd",
                newName: "Region");
        }
    }
}
