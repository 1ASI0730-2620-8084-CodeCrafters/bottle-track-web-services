using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIotMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "iot_devices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    serial_number = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    safe_temperature_min = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    safe_temperature_max = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    impact_threshold = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_iot_devices", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    iot_device_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    transport_operation_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    raised_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    acknowledged_by = table.Column<Guid>(type: "char(36)", nullable: true),
                    acknowledged_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alerts", x => x.id);
                    table.ForeignKey(
                        name: "f_k_alerts__iot_device_iot_device_id",
                        column: x => x.iot_device_id,
                        principalTable: "iot_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensor_readings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    iot_device_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    transport_operation_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    recorded_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    temperature_celsius = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    impact_force = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensor_readings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_sensor_readings_iot_devices_iot_device_id",
                        column: x => x.iot_device_id,
                        principalTable: "iot_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_iot_device_id",
                table: "alerts",
                column: "iot_device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_iot_devices_serial_number",
                table: "iot_devices",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sensor_readings_iot_device_id",
                table: "sensor_readings",
                column: "iot_device_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "sensor_readings");

            migrationBuilder.DropTable(
                name: "iot_devices");
        }
    }
}
