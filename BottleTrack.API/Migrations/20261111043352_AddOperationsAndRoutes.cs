using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationsAndRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_points",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    organization_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    business_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    district = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_delivery_points", x => x.id);
                    table.ForeignKey(
                        name: "f_k_delivery_points_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "transport_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    organization_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    scheduled_date = table.Column<DateOnly>(type: "date", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    driver_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    dispatched_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cancellation_reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_transport_operations", x => x.id);
                    table.ForeignKey(
                        name: "f_k_transport_operations_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "delivery_stops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    transport_operation_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    delivery_point_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    sequence_number = table.Column<int>(type: "int", nullable: false),
                    planned_quantity = table.Column<int>(type: "int", nullable: false),
                    delivered_quantity = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    failure_reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    confirmed_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_delivery_stops", x => x.id);
                    table.ForeignKey(
                        name: "f_k_delivery_stops_delivery_points_delivery_point_id",
                        column: x => x.delivery_point_id,
                        principalTable: "delivery_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_delivery_stops_transport_operations_transport_operation_id",
                        column: x => x.transport_operation_id,
                        principalTable: "transport_operations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_delivery_points_organization_id",
                table: "delivery_points",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "i_x_delivery_stops_delivery_point_id",
                table: "delivery_stops",
                column: "delivery_point_id");

            migrationBuilder.CreateIndex(
                name: "i_x_delivery_stops_transport_operation_id",
                table: "delivery_stops",
                column: "transport_operation_id");

            migrationBuilder.CreateIndex(
                name: "i_x_transport_operations_code",
                table: "transport_operations",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_transport_operations_organization_id",
                table: "transport_operations",
                column: "organization_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_stops");

            migrationBuilder.DropTable(
                name: "delivery_points");

            migrationBuilder.DropTable(
                name: "transport_operations");
        }
    }
}
