using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class FixScheduledDateColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "scheduled_date",
                table: "transport_operations",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "scheduled_date",
                table: "transport_operations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
