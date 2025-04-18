using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvin.IDP.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSecrets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSecrets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecrets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("41f72664-5378-426b-8871-0b8cb852bf22"),
                column: "ConcurrencyStamp",
                value: "cebde8b2-96ae-4bba-89ee-733e2c6b98fb");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("4a337664-2555-40c6-aad6-53aab772d766"),
                column: "ConcurrencyStamp",
                value: "dbc0f045-b83d-4751-9b86-cbefbfcd5ef2");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("508b5a3c-ad01-4376-ade9-2aae53990c0b"),
                column: "ConcurrencyStamp",
                value: "2004188f-e146-4651-b4b3-4c3dadd80e7c");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("63647ad2-2557-4b0b-9065-f95bfbcd07cb"),
                column: "ConcurrencyStamp",
                value: "20a832cc-5aeb-492e-93e7-c327753804a1");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("772648ab-ff21-4684-a959-f7cc55633c70"),
                column: "ConcurrencyStamp",
                value: "8baf7fa2-cec0-4f09-afe6-684cab56c017");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("961a83d0-1263-4b96-899b-059e168b03a5"),
                column: "ConcurrencyStamp",
                value: "3d02e5f0-f839-458d-8142-95b412f3a02d");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a15ee06c-a2fe-4036-8a70-63eb4b7dc646"),
                column: "ConcurrencyStamp",
                value: "a6cc888d-fb77-41f2-8cf3-41755b769bc4");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ccca788f-bdec-4d9f-9caf-b149616d6867"),
                column: "ConcurrencyStamp",
                value: "fb78a965-b1ba-4f0b-a251-3458eb3767fd");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecrets_UserId",
                table: "UserSecrets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSecrets");

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
    }
}
