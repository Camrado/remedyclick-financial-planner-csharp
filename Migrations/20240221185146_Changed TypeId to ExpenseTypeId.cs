using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace financial_planner.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTypeIdtoExpenseTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseType_TypeId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Expenses",
                newName: "ExpenseTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_TypeId",
                table: "Expenses",
                newName: "IX_Expenses_ExpenseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseType_ExpenseTypeId",
                table: "Expenses",
                column: "ExpenseTypeId",
                principalTable: "ExpenseType",
                principalColumn: "ExpenseTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseType_ExpenseTypeId",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "ExpenseTypeId",
                table: "Expenses",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_ExpenseTypeId",
                table: "Expenses",
                newName: "IX_Expenses_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseType_TypeId",
                table: "Expenses",
                column: "TypeId",
                principalTable: "ExpenseType",
                principalColumn: "ExpenseTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
