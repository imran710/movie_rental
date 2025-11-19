using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rental_models_type_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalItems_Movies_MovieId1",
                table: "RentalItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_CustomerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_RentalItems_MovieId1",
                table: "RentalItems");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "RentalItems");

            migrationBuilder.AlterColumn<long>(
                name: "userid",
                table: "Rentals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "RentalItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<long>(
                name: "CreationInfo_CreatedBy",
                table: "Movies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_userid",
                table: "Rentals",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_RentalItems_MovieId",
                table: "RentalItems",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalItems_Movies_MovieId",
                table: "RentalItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_userid",
                table: "Rentals",
                column: "userid",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalItems_Movies_MovieId",
                table: "RentalItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Users_userid",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_userid",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_RentalItems_MovieId",
                table: "RentalItems");

            migrationBuilder.DropColumn(
                name: "CreationInfo_CreatedBy",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "Rentals",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "Rentals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "RentalItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "RentalItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalItems_MovieId1",
                table: "RentalItems",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalItems_Movies_MovieId1",
                table: "RentalItems",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Users_CustomerId",
                table: "Rentals",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
