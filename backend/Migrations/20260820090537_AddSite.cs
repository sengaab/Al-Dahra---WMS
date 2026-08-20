using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace whm.Migrations
{
    /// <inheritdoc />
    public partial class AddSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ==========================================
            // 1. Create Sites table
            // ==========================================

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Site_Id = table.Column<int>(
                        type: "integer",
                        nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    Site_Name = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false),

                    Site_Code = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true),

                    Site_Description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true),

                    IsActive = table.Column<bool>(
                        type: "boolean",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_Sites",
                        x => x.Site_Id);
                });


            // ==========================================
            // 2. Create default Site
            // ==========================================

            migrationBuilder.InsertData(
                table: "Sites",
                columns: new[]
                {
                    "Site_Name",
                    "Site_Code",
                    "Site_Description",
                    "IsActive"
                },
                values: new object[]
                {
                    "Main Site",
                    "SITE001",
                    "Default site for existing warehouses",
                    true
                });


            // ==========================================
            // 3. Add Site_Id to existing Warehouses
            // ==========================================

            migrationBuilder.AddColumn<int>(
                name: "Site_Id",
                table: "Warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 1);


            // ==========================================
            // 4. Create Index
            // ==========================================

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Site_Id",
                table: "Warehouses",
                column: "Site_Id");


            // ==========================================
            // 5. Create Foreign Key
            // ==========================================

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Sites_Site_Id",
                table: "Warehouses",
                column: "Site_Id",
                principalTable: "Sites",
                principalColumn: "Site_Id",
                onDelete: ReferentialAction.Restrict);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Sites_Site_Id",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_Site_Id",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Site_Id",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}