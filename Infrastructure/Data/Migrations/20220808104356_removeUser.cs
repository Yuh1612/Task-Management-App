using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class removeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("0598fc66-4ec4-45f5-a2cf-52ff729acd20"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("066834ef-807b-4414-b9b5-253bf1abd38d"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("2b225e43-f924-4514-b125-b001f68d4822"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("3c08448c-fc93-4c5a-8c34-e48ddd7cf12d"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("4405f6b0-3019-4095-8441-aafcc8d36608"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("b025ff2d-4d33-4a84-a49f-c228eb795f55"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("c61e3da3-6b1a-4ec2-a229-b735864511ad"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("d09bdfa2-6e43-4b06-bb16-88fa5ea50d27"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("d165ee37-a62b-4a9a-a418-bad940652cd7"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("ed10a4a5-0f83-482f-b3ed-4b0156017606"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("f923ce44-176b-45f5-972f-25b216a7b93f"));

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiredDay",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "Color", "IsDelete", "Title" },
                values: new object[,]
                {
                    { new Guid("0fd4e040-6b5b-4da5-a7b5-2b2f9a5654b6"), "White", false, "VueJs" },
                    { new Guid("1ce67fd5-3efe-490c-80dd-5fc1f46e551e"), "Black", false, "ReactJs" },
                    { new Guid("254eded3-591b-46c5-b863-183459fc5520"), "Red", false, "Angular" },
                    { new Guid("2be83ce3-d40d-4dc1-8f6c-09e41ad71a7e"), "Purple", false, ".NET" },
                    { new Guid("52c9efc1-7797-4542-a96d-7ae1e7e680a1"), "Green", false, "NestJs" },
                    { new Guid("547628fd-c42f-4e1f-8e38-b32d6a3d8743"), "Blue", false, "Java" },
                    { new Guid("89e3a897-d2f6-4b66-9eeb-571371d4c729"), "Gray", false, "Dart" },
                    { new Guid("a35236cf-262f-4b07-b5da-38f71e37912c"), "Brown", false, "Golang" },
                    { new Guid("a579813f-4283-4b00-9f33-d08f9db53e40"), "Orange", false, "Python" },
                    { new Guid("c80750b9-29bc-4623-9cd7-a32cde9ff522"), "Pink", false, "PHP" },
                    { new Guid("f66d81e0-8b78-4ccf-8382-e12a9b323e55"), "Black", false, "Flutter" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("0fd4e040-6b5b-4da5-a7b5-2b2f9a5654b6"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("1ce67fd5-3efe-490c-80dd-5fc1f46e551e"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("254eded3-591b-46c5-b863-183459fc5520"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("2be83ce3-d40d-4dc1-8f6c-09e41ad71a7e"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("52c9efc1-7797-4542-a96d-7ae1e7e680a1"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("547628fd-c42f-4e1f-8e38-b32d6a3d8743"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("89e3a897-d2f6-4b66-9eeb-571371d4c729"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("a35236cf-262f-4b07-b5da-38f71e37912c"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("a579813f-4283-4b00-9f33-d08f9db53e40"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("c80750b9-29bc-4623-9cd7-a32cde9ff522"));

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: new Guid("f66d81e0-8b78-4ccf-8382-e12a9b323e55"));

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiredDay",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "Color", "IsDelete", "Title" },
                values: new object[,]
                {
                    { new Guid("0598fc66-4ec4-45f5-a2cf-52ff729acd20"), "White", false, "VueJs" },
                    { new Guid("066834ef-807b-4414-b9b5-253bf1abd38d"), "Black", false, "ReactJs" },
                    { new Guid("2b225e43-f924-4514-b125-b001f68d4822"), "Pink", false, "PHP" },
                    { new Guid("3c08448c-fc93-4c5a-8c34-e48ddd7cf12d"), "Blue", false, "Java" },
                    { new Guid("4405f6b0-3019-4095-8441-aafcc8d36608"), "Black", false, "Flutter" },
                    { new Guid("b025ff2d-4d33-4a84-a49f-c228eb795f55"), "Gray", false, "Dart" },
                    { new Guid("c61e3da3-6b1a-4ec2-a229-b735864511ad"), "Orange", false, "Python" },
                    { new Guid("d09bdfa2-6e43-4b06-bb16-88fa5ea50d27"), "Green", false, "NestJs" },
                    { new Guid("d165ee37-a62b-4a9a-a418-bad940652cd7"), "Red", false, "Angular" },
                    { new Guid("ed10a4a5-0f83-482f-b3ed-4b0156017606"), "Brown", false, "Golang" },
                    { new Guid("f923ce44-176b-45f5-972f-25b216a7b93f"), "Purple", false, ".NET" }
                });
        }
    }
}
