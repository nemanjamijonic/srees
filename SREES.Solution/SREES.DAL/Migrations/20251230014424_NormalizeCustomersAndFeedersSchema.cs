using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SREES.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeCustomersAndFeedersSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Buildings_BuildingId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Feeders_Substations_SubstationId",
                table: "Feeders");

            migrationBuilder.RenameTable(
                name: "Feeders",
                newName: "Feeders",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customers",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Feeders_SubstationId",
                schema: "dbo",
                table: "Feeders",
                newName: "IX_Feeder_SubstationId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_BuildingId",
                schema: "dbo",
                table: "Customers",
                newName: "IX_Customer_BuildingId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Feeders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                schema: "dbo",
                table: "Feeders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Feeders",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "dbo",
                table: "Feeders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Feeders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                schema: "dbo",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "dbo",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Customers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Feeder_IsDeleted",
                schema: "dbo",
                table: "Feeders",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Feeder_Type",
                schema: "dbo",
                table: "Feeders",
                column: "FeederType");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IsActive",
                schema: "dbo",
                table: "Customers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IsDeleted",
                schema: "dbo",
                table: "Customers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Type",
                schema: "dbo",
                table: "Customers",
                column: "CustomerType");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Buildings_BuildingId",
                schema: "dbo",
                table: "Customers",
                column: "BuildingId",
                principalSchema: "dbo",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Feeders_Substations_SubstationId",
                schema: "dbo",
                table: "Feeders",
                column: "SubstationId",
                principalSchema: "dbo",
                principalTable: "Substations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Buildings_BuildingId",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Feeders_Substations_SubstationId",
                schema: "dbo",
                table: "Feeders");

            migrationBuilder.DropIndex(
                name: "IX_Feeder_IsDeleted",
                schema: "dbo",
                table: "Feeders");

            migrationBuilder.DropIndex(
                name: "IX_Feeder_Type",
                schema: "dbo",
                table: "Feeders");

            migrationBuilder.DropIndex(
                name: "IX_Customer_IsActive",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customer_IsDeleted",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Type",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Feeders",
                schema: "dbo",
                newName: "Feeders");

            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "dbo",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Feeder_SubstationId",
                table: "Feeders",
                newName: "IX_Feeders_SubstationId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_BuildingId",
                table: "Customers",
                newName: "IX_Customers_BuildingId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Feeders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Feeders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Feeders",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Feeders",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Feeders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Customers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Buildings_BuildingId",
                table: "Customers",
                column: "BuildingId",
                principalSchema: "dbo",
                principalTable: "Buildings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeders_Substations_SubstationId",
                table: "Feeders",
                column: "SubstationId",
                principalSchema: "dbo",
                principalTable: "Substations",
                principalColumn: "Id");
        }
    }
}
