using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SREES.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPoleTypeFromStringToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poles_Regions_RegionId",
                table: "Poles");

            migrationBuilder.RenameTable(
                name: "Poles",
                newName: "Poles",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Poles_RegionId",
                schema: "dbo",
                table: "Poles",
                newName: "IX_Pole_RegionId");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                schema: "dbo",
                table: "Poles",
                type: "float(10)",
                precision: 10,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                schema: "dbo",
                table: "Poles",
                type: "float(10)",
                precision: 10,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                schema: "dbo",
                table: "Poles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Poles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "dbo",
                table: "Poles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Poles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Poles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pole_IsDeleted",
                schema: "dbo",
                table: "Poles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Pole_Type",
                schema: "dbo",
                table: "Poles",
                column: "PoleType");

            migrationBuilder.AddForeignKey(
                name: "FK_Poles_Regions_RegionId",
                schema: "dbo",
                table: "Poles",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poles_Regions_RegionId",
                schema: "dbo",
                table: "Poles");

            migrationBuilder.DropIndex(
                name: "IX_Pole_IsDeleted",
                schema: "dbo",
                table: "Poles");

            migrationBuilder.DropIndex(
                name: "IX_Pole_Type",
                schema: "dbo",
                table: "Poles");

            migrationBuilder.RenameTable(
                name: "Poles",
                schema: "dbo",
                newName: "Poles");

            migrationBuilder.RenameIndex(
                name: "IX_Pole_RegionId",
                table: "Poles",
                newName: "IX_Poles_RegionId");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Poles",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Poles",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Poles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Poles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Poles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Poles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Poles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Poles_Regions_RegionId",
                table: "Poles",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}
