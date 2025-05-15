using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Product.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Category", "Code", "CreatedDate", "ImageURL", "IsDeleted", "Name", "Price", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("39ecb0f9-e945-48ea-9880-8400d61cb7c9"), "Phone", "Iphone13White", new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Local), "https://productimages.hepsiburada.net/s/189/424-600/110000155170588.jpg/format:webp", false, "Iphone 13", 34299.0m, new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Local) },
                    { new Guid("60742fba-3da2-42ec-931a-7e7edd77d3a5"), "Phone", "Iphone15White", new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Local), "https://productimages.hepsiburada.net/s/462/424-600/110000498573428.jpg/format:webp", false, "Iphone 15", 48299.0m, new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Local) },
                    { new Guid("63870905-124c-41be-9827-a9e026172fb6"), "Phone", "Iphone14White", new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Local), "https://productimages.hepsiburada.net/s/376/424-600/110000393677091.jpg/format:webp", false, "Iphone 14", 38299.0m, new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Local) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Code",
                table: "Product",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
