using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvin.IDP.Migrations
{
    /// <inheritdoc />
    public partial class AddUsrLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ProviderIdentityKey = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
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
                value: "096dff14-764e-42f4-9202-6ba700bb6ecb");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("4a337664-2555-40c6-aad6-53aab772d766"),
                column: "ConcurrencyStamp",
                value: "1958b2ec-74d0-4be1-87d9-6daaaca82f64");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("508b5a3c-ad01-4376-ade9-2aae53990c0b"),
                column: "ConcurrencyStamp",
                value: "7807a64c-7776-42d1-9d92-3e6c86331859");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("63647ad2-2557-4b0b-9065-f95bfbcd07cb"),
                column: "ConcurrencyStamp",
                value: "98c77f0d-3a95-44b1-94f0-5567e7b54ba5");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("772648ab-ff21-4684-a959-f7cc55633c70"),
                column: "ConcurrencyStamp",
                value: "725b0313-4980-4feb-be15-ca44e423c91e");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("961a83d0-1263-4b96-899b-059e168b03a5"),
                column: "ConcurrencyStamp",
                value: "0266f655-7082-4102-aafd-8714b39a5c76");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a15ee06c-a2fe-4036-8a70-63eb4b7dc646"),
                column: "ConcurrencyStamp",
                value: "4b5df728-4934-4bcc-b901-cc7d5abe39b4");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ccca788f-bdec-4d9f-9caf-b149616d6867"),
                column: "ConcurrencyStamp",
                value: "9adcb458-3fc7-422b-80c7-d4024535f764");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("41f72664-5378-426b-8871-0b8cb852bf22"),
                column: "ConcurrencyStamp",
                value: "daca210e-f48d-4f8e-93e4-b5b3e051877d");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("4a337664-2555-40c6-aad6-53aab772d766"),
                column: "ConcurrencyStamp",
                value: "25468fce-87d2-484a-864d-6dbfd8dc4424");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("508b5a3c-ad01-4376-ade9-2aae53990c0b"),
                column: "ConcurrencyStamp",
                value: "5f51296d-1642-47c4-9bb6-330a1b28382a");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("63647ad2-2557-4b0b-9065-f95bfbcd07cb"),
                column: "ConcurrencyStamp",
                value: "33b29226-253f-4e5d-8913-af90aff8f11c");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("772648ab-ff21-4684-a959-f7cc55633c70"),
                column: "ConcurrencyStamp",
                value: "be1352da-c81b-481a-85d5-fc614bd9af6e");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("961a83d0-1263-4b96-899b-059e168b03a5"),
                column: "ConcurrencyStamp",
                value: "530b39a4-56af-4f4d-a494-9e57046e7f39");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a15ee06c-a2fe-4036-8a70-63eb4b7dc646"),
                column: "ConcurrencyStamp",
                value: "e699ee7c-0537-4285-a696-148126b64249");

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("ccca788f-bdec-4d9f-9caf-b149616d6867"),
                column: "ConcurrencyStamp",
                value: "ec48b674-6479-4f09-a84b-4ee185f77a08");
        }
    }
}
