using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace financial_planner.Migrations
{
    /// <inheritdoc />
    public partial class AddedExpenseTypesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseType_ExpenseTypeId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseType",
                table: "ExpenseType");

            migrationBuilder.RenameTable(
                name: "ExpenseType",
                newName: "ExpenseTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseTypes",
                table: "ExpenseTypes",
                column: "ExpenseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseTypes_ExpenseTypeId",
                table: "Expenses",
                column: "ExpenseTypeId",
                principalTable: "ExpenseTypes",
                principalColumn: "ExpenseTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseTypes_ExpenseTypeId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseTypes",
                table: "ExpenseTypes");

            migrationBuilder.RenameTable(
                name: "ExpenseTypes",
                newName: "ExpenseType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseType",
                table: "ExpenseType",
                column: "ExpenseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseType_ExpenseTypeId",
                table: "Expenses",
                column: "ExpenseTypeId",
                principalTable: "ExpenseType",
                principalColumn: "ExpenseTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
