using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SREES.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedLatLongToFeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Regions_RegionId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetectedFeederId",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetectedLevel",
                schema: "dbo",
                table: "Outages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetectedPoleId",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetectedSubstationId",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoDetected",
                schema: "dbo",
                table: "Outages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutageGroupId",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                schema: "dbo",
                table: "Outages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReportedAddress",
                schema: "dbo",
                table: "Outages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReportedLatitude",
                schema: "dbo",
                table: "Outages",
                type: "float(10)",
                precision: 10,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReportedLongitude",
                schema: "dbo",
                table: "Outages",
                type: "float(10)",
                precision: 10,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Severity",
                schema: "dbo",
                table: "Outages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "dbo",
                table: "Feeders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "dbo",
                table: "Feeders",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OutageGroups",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DetectedLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AffectedCustomersCount = table.Column<int>(type: "int", nullable: false),
                    DetectedSubstationId = table.Column<int>(type: "int", nullable: true),
                    DetectedFeederId = table.Column<int>(type: "int", nullable: true),
                    DetectedPoleId = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutageGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutageGroups_Feeders_DetectedFeederId",
                        column: x => x.DetectedFeederId,
                        principalSchema: "dbo",
                        principalTable: "Feeders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OutageGroups_Poles_DetectedPoleId",
                        column: x => x.DetectedPoleId,
                        principalSchema: "dbo",
                        principalTable: "Poles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OutageGroups_Regions_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "dbo",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutageGroups_Substations_DetectedSubstationId",
                        column: x => x.DetectedSubstationId,
                        principalSchema: "dbo",
                        principalTable: "Substations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outage_GroupId",
                schema: "dbo",
                table: "Outages",
                column: "OutageGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Outages_BuildingId",
                schema: "dbo",
                table: "Outages",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Outages_CustomerId",
                schema: "dbo",
                table: "Outages",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Outages_DetectedFeederId",
                schema: "dbo",
                table: "Outages",
                column: "DetectedFeederId");

            migrationBuilder.CreateIndex(
                name: "IX_Outages_DetectedPoleId",
                schema: "dbo",
                table: "Outages",
                column: "DetectedPoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Outages_DetectedSubstationId",
                schema: "dbo",
                table: "Outages",
                column: "DetectedSubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroup_IsDeleted",
                schema: "dbo",
                table: "OutageGroups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroup_IsResolved",
                schema: "dbo",
                table: "OutageGroups",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroup_RegionId",
                schema: "dbo",
                table: "OutageGroups",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroup_Severity",
                schema: "dbo",
                table: "OutageGroups",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroups_DetectedFeederId",
                schema: "dbo",
                table: "OutageGroups",
                column: "DetectedFeederId");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroups_DetectedPoleId",
                schema: "dbo",
                table: "OutageGroups",
                column: "DetectedPoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OutageGroups_DetectedSubstationId",
                schema: "dbo",
                table: "OutageGroups",
                column: "DetectedSubstationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Buildings_BuildingId",
                schema: "dbo",
                table: "Outages",
                column: "BuildingId",
                principalSchema: "dbo",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Customers_CustomerId",
                schema: "dbo",
                table: "Outages",
                column: "CustomerId",
                principalSchema: "dbo",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Feeders_DetectedFeederId",
                schema: "dbo",
                table: "Outages",
                column: "DetectedFeederId",
                principalSchema: "dbo",
                principalTable: "Feeders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_OutageGroups_OutageGroupId",
                schema: "dbo",
                table: "Outages",
                column: "OutageGroupId",
                principalSchema: "dbo",
                principalTable: "OutageGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Poles_DetectedPoleId",
                schema: "dbo",
                table: "Outages",
                column: "DetectedPoleId",
                principalSchema: "dbo",
                principalTable: "Poles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Regions_RegionId",
                schema: "dbo",
                table: "Outages",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Substations_DetectedSubstationId",
                schema: "dbo",
                table: "Outages",
                column: "DetectedSubstationId",
                principalSchema: "dbo",
                principalTable: "Substations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Buildings_BuildingId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Customers_CustomerId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Feeders_DetectedFeederId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropForeignKey(
                name: "FK_Outages_OutageGroups_OutageGroupId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Poles_DetectedPoleId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Regions_RegionId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropForeignKey(
                name: "FK_Outages_Substations_DetectedSubstationId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropTable(
                name: "OutageGroups",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Outage_GroupId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropIndex(
                name: "IX_Outages_BuildingId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropIndex(
                name: "IX_Outages_CustomerId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropIndex(
                name: "IX_Outages_DetectedFeederId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropIndex(
                name: "IX_Outages_DetectedPoleId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropIndex(
                name: "IX_Outages_DetectedSubstationId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "DetectedFeederId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "DetectedLevel",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "DetectedPoleId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "DetectedSubstationId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "IsAutoDetected",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "OutageGroupId",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "ReportedAddress",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "ReportedLatitude",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "ReportedLongitude",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "Severity",
                schema: "dbo",
                table: "Outages");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "dbo",
                table: "Feeders");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "dbo",
                table: "Feeders");

            migrationBuilder.AddForeignKey(
                name: "FK_Outages_Regions_RegionId",
                schema: "dbo",
                table: "Outages",
                column: "RegionId",
                principalSchema: "dbo",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
