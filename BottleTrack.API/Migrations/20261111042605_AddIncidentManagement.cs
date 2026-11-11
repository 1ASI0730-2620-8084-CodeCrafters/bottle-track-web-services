using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIncidentManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "incidents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    transport_operation_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    delivery_stop_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    reported_by = table.Column<Guid>(type: "char(36)", nullable: false),
                    source = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    affected_quantity = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    resolution = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    reported_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    resolved_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incidents", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "evidences",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    incident_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    file_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    file_type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    uploaded_by = table.Column<Guid>(type: "char(36)", nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_evidences", x => x.id);
                    table.ForeignKey(
                        name: "f_k_evidences_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_evidences_incident_id",
                table: "evidences",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_code",
                table: "incidents",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "evidences");

            migrationBuilder.DropTable(
                name: "incidents");
        }
    }
}
