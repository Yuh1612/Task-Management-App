using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class SixthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "Color", "IsDelete", "Title" },
                values: new object[,]
                {
                    { new Guid("2d32c7a7-2a0d-4cae-8cfe-78f2b18e33b8"), "Gray", false, "Dart" },
                    { new Guid("428cf5c2-96a4-4b09-bc02-d2c6ada686ba"), "Pink", false, "PHP" },
                    { new Guid("73ed1499-b9f6-4e2a-b01e-bf446250d98c"), "Red", false, "Angular" },
                    { new Guid("83df3d45-4c5a-402b-a7ac-2fc7bb4e1bf5"), "Black", false, "Flutter" },
                    { new Guid("9a1f1f40-b5ec-4a4d-9eca-ef42e5c0671a"), "Black", false, "ReactJs" },
                    { new Guid("9e5e3460-e1e5-4f0c-aab3-f13691314093"), "Green", false, "NestJs" },
                    { new Guid("9e765f9f-72a2-490b-a834-73b1c0d8c8bc"), "Purple", false, ".NET" },
                    { new Guid("b270569a-b9de-48c6-8014-0721b1c3744d"), "Orange", false, "Python" },
                    { new Guid("c8dbb5c4-c9ce-4e73-b0f7-d66bd07e4a96"), "White", false, "VueJs" },
                    { new Guid("ccc50436-d056-4723-9bd5-492ab3a792d0"), "Brown", false, "Golang" },
                    { new Guid("fb6ac416-d11a-427b-82e1-0cd94fbdded7"), "Blue", false, "Java" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("2d32c7a7-2a0d-4cae-8cfe-78f2b18e33b8"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("428cf5c2-96a4-4b09-bc02-d2c6ada686ba"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("73ed1499-b9f6-4e2a-b01e-bf446250d98c"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("83df3d45-4c5a-402b-a7ac-2fc7bb4e1bf5"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("9a1f1f40-b5ec-4a4d-9eca-ef42e5c0671a"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("9e5e3460-e1e5-4f0c-aab3-f13691314093"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("9e765f9f-72a2-490b-a834-73b1c0d8c8bc"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("b270569a-b9de-48c6-8014-0721b1c3744d"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("c8dbb5c4-c9ce-4e73-b0f7-d66bd07e4a96"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("ccc50436-d056-4723-9bd5-492ab3a792d0"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("fb6ac416-d11a-427b-82e1-0cd94fbdded7"));
        }
    }
}