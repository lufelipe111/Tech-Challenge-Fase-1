using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactRegister.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContactDddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_contact_tb_ddd_DddCodeId",
                table: "tb_contact");

            migrationBuilder.RenameColumn(
                name: "DddCodeId",
                table: "tb_contact",
                newName: "ddd_id");

            migrationBuilder.RenameIndex(
                name: "IX_tb_contact_DddCodeId",
                table: "tb_contact",
                newName: "IX_tb_contact_ddd_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_contact_tb_ddd_ddd_id",
                table: "tb_contact",
                column: "ddd_id",
                principalTable: "tb_ddd",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_contact_tb_ddd_ddd_id",
                table: "tb_contact");

            migrationBuilder.RenameColumn(
                name: "ddd_id",
                table: "tb_contact",
                newName: "DddCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_contact_ddd_id",
                table: "tb_contact",
                newName: "IX_tb_contact_DddCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_contact_tb_ddd_DddCodeId",
                table: "tb_contact",
                column: "DddCodeId",
                principalTable: "tb_ddd",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
