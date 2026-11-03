using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BottleTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    business_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    tax_id = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_organizations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    organization_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    first_name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    last_name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    document_number = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    license_number = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false),
                    phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_drivers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_drivers__organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    organization_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    first_name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    last_name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                    table.ForeignKey(
                        name: "f_k_users_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    organization_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    plate_number = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false),
                    brand = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    model = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    capacity_in_boxes = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "f_k_vehicles__organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_drivers_document_number",
                table: "drivers",
                column: "document_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_drivers_license_number",
                table: "drivers",
                column: "license_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_drivers_organization_id",
                table: "drivers",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "i_x_organizations_tax_id",
                table: "organizations",
                column: "tax_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_users_organization_id",
                table: "users",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_organization_id",
                table: "vehicles",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_plate_number",
                table: "vehicles",
                column: "plate_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "organizations");
        }
    }
}
