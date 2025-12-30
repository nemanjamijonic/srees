using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SREES.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedBuildingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Poles_PoleId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Regions_RegionId",
                table: "Buildings");

            migrationBuilder.RenameTable(
                name: "Buildings",
                newName: "Buildings",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Buildings_RegionId",
                schema: "dbo",
                table: "Buildings",
                newName: "IX_Building_RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Buildings_PoleId",
                schema: "dbo",
                table: "Buildings",
                newName: "IX_Building_PoleId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerName",
                schema: "dbo",
                table: "Buildings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                schema: "dbo",
                table: "Buildings",
                type: "float(10)",
                precision: 10,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                schema: "dbo",
                table: "Buildings",
                type: "float(10)",
                precision: 10,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                schema: "dbo",
                table: "Buildings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Buildings",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                schema: "dbo",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Buildings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Buildings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Building_IsDeleted",
                schema: "dbo",
                table: "Buildings",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Poles_PoleId",
                schema: "dbo",
                table: "Buildings",
                column: "PoleId",
                principalSchema: "dbo",
                principalTable: "Poles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Regions_RegionId",
                schema: "dbo",
                table: "Buildings",
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
                name: "FK_Buildings_Poles_PoleId",
                schema: "dbo",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Regions_RegionId",
                schema: "dbo",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Building_IsDeleted",
                schema: "dbo",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Buildings",
                schema: "dbo",
                newName: "Buildings");

            migrationBuilder.RenameIndex(
                name: "IX_Building_RegionId",
                table: "Buildings",
                newName: "IX_Buildings_RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Building_PoleId",
                table: "Buildings",
                newName: "IX_Buildings_PoleId");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerName",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Buildings",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Buildings",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(10)",
                oldPrecision: 10,
                oldScale: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Buildings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Buildings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Buildings",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Buildings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Poles_PoleId",
                table: "Buildings",
                column: "PoleId",
                principalSchema: "dbo",
                principalTable: "Poles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Regions_RegionId",
                table: "Buildings",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}
