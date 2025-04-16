using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageGallery.API.Migrations
{
    /// <inheritdoc />
    public partial class ImageToAdUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "FileName", "OwnerId", "Title" },
                values: new object[] { new Guid("2d51ebee-f9ed-4335-8fb2-b6e63bae250d"), "fdfe7329-e05c-41fb-a7c7-4f3226d28c49.jpg", "AAC8CD3F-E3AA-4BA5-988C-649C1B586387", "An image by celso" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("2d51ebee-f9ed-4335-8fb2-b6e63bae250d"));
        }
    }
}
