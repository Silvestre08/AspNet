using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvin.IDP.Migrations
{
    /// <inheritdoc />
    public partial class ConcurrencyCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("41f72664-5378-426b-8871-0b8cb852bf22"),
                column: "ConcurrencyStamp",
                value: "6a124ff2-1773-4f29-9ef0-b5ee240955de");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("4a337664-2555-40c6-aad6-53aab772d766"),
                column: "ConcurrencyStamp",
                value: "457a3fff-9636-4a57-9165-5aecb675d914");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("508b5a3c-ad01-4376-ade9-2aae53990c0b"),
                column: "ConcurrencyStamp",
                value: "cec1e269-0105-4e10-ac68-f5748b897909");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("63647ad2-2557-4b0b-9065-f95bfbcd07cb"),
                column: "ConcurrencyStamp",
                value: "015d3401-fb9f-4d4e-88f3-af93b0cab328");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("772648ab-ff21-4684-a959-f7cc55633c70"),
                column: "ConcurrencyStamp",
                value: "52f91473-d5e2-43db-a377-d33f226fab13");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("961a83d0-1263-4b96-899b-059e168b03a5"),
                column: "ConcurrencyStamp",
                value: "4a25b837-4e64-46d7-ab22-1fb1822d6a72");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a15ee06c-a2fe-4036-8a70-63eb4b7dc646"),
                column: "ConcurrencyStamp",
                value: "380a3178-e1a7-4e5b-8c10-9f756dc83991");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ccca788f-bdec-4d9f-9caf-b149616d6867"),
                column: "ConcurrencyStamp",
                value: "6b041387-5a38-4a3c-8650-f79c7454641a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("41f72664-5378-426b-8871-0b8cb852bf22"),
                column: "ConcurrencyStamp",
                value: "80bdc229-3510-49a5-b303-ad4aecf9534c");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("4a337664-2555-40c6-aad6-53aab772d766"),
                column: "ConcurrencyStamp",
                value: "8ef69d45-7fff-4444-8546-ab41e8f96273");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("508b5a3c-ad01-4376-ade9-2aae53990c0b"),
                column: "ConcurrencyStamp",
                value: "96c4e708-c6c4-47e2-92a6-e87963436938");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("63647ad2-2557-4b0b-9065-f95bfbcd07cb"),
                column: "ConcurrencyStamp",
                value: "73666019-5e84-4e7c-9031-cd2f413616df");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("772648ab-ff21-4684-a959-f7cc55633c70"),
                column: "ConcurrencyStamp",
                value: "ead86fe6-9deb-4ba6-9a56-5207b7591c70");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("961a83d0-1263-4b96-899b-059e168b03a5"),
                column: "ConcurrencyStamp",
                value: "0f9ae755-7ac7-43ca-97b5-9ff9121cd778");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a15ee06c-a2fe-4036-8a70-63eb4b7dc646"),
                column: "ConcurrencyStamp",
                value: "2ba2e25e-f0dc-4a7a-9ea7-0a05aaff5f95");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ccca788f-bdec-4d9f-9caf-b149616d6867"),
                column: "ConcurrencyStamp",
                value: "f971f570-5543-4006-b2a3-523f1ebbd914");
        }
    }
}
