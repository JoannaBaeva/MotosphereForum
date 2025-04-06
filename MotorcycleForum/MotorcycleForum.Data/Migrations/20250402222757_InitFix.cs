using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2845ef41-e141-466a-8793-df98564b82b6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("536efe1f-e60a-4891-bab2-2f4ffab7f464"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7ae425ce-e1ab-414e-adec-b8cd27a19569"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("39727667-1f7b-488f-8560-1e2942777b94"), null, "Moderator", "MODERATOR" },
                    { new Guid("711392e6-c020-463d-8a42-01ef90dd6273"), null, "User", "USER" },
                    { new Guid("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ConcurrencyStamp", "FullName", "RegistrationDate" },
                values: new object[] { "f366a811-cf37-4a43-b881-bc750f9368ec", "MotosphereAdmin", new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8572) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8884));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8868));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("39727667-1f7b-488f-8560-1e2942777b94"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("711392e6-c020-463d-8a42-01ef90dd6273"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2845ef41-e141-466a-8793-df98564b82b6"), null, "Admin", "ADMIN" },
                    { new Guid("536efe1f-e60a-4891-bab2-2f4ffab7f464"), null, "Moderator", "MODERATOR" },
                    { new Guid("7ae425ce-e1ab-414e-adec-b8cd27a19569"), null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ConcurrencyStamp", "FullName", "RegistrationDate" },
                values: new object[] { "db418197-7449-4747-aede-4a292713b3dc", null, new DateTime(2025, 4, 2, 22, 8, 14, 155, DateTimeKind.Utc).AddTicks(176) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 8, 14, 155, DateTimeKind.Utc).AddTicks(478));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 8, 14, 155, DateTimeKind.Utc).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 8, 14, 155, DateTimeKind.Utc).AddTicks(438));
        }
    }
}
