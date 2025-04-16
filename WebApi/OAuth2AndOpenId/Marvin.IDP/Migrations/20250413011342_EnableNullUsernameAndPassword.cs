using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvin.IDP.Migrations
{
    /// <inheritdoc />
    public partial class EnableNullUsernameAndPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

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
        }
    }
}
