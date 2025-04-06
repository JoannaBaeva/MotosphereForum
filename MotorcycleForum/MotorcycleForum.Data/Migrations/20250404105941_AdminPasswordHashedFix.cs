using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdminPasswordHashedFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "e1cb31c4-fcd4-47d7-b1cb-9c7e9edcdd70", "AQAAAAEAACcQAAAAEE3gPzXxHcxTe7u7VOiKtKFc9kg4vhToJkFVz8PGVJPGW26T52YbKmjaDE5ZljzPuw==", new DateTime(2025, 4, 4, 10, 59, 41, 383, DateTimeKind.Utc).AddTicks(5761), "2f3c8b65-12af-4b6d-bda9-8ec3d8651d3a" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 59, 41, 383, DateTimeKind.Utc).AddTicks(6106));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 59, 41, 383, DateTimeKind.Utc).AddTicks(6085));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 59, 41, 383, DateTimeKind.Utc).AddTicks(6068));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate", "SecurityStamp" },
                values: new object[] { "ddf98934-8845-4bbb-a1ab-ac8ed402c436", "X;W0Q6^Ej0Xc", new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1626), null });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1906));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1886));
        }
    }
}
