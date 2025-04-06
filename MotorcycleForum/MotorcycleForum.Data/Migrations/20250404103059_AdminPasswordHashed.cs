using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdminPasswordHashed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate" },
                values: new object[] { "ddf98934-8845-4bbb-a1ab-ac8ed402c436", "X;W0Q6^Ej0Xc", new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1626) });

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
                columns: new[] { "Content", "CreatedDate" },
                values: new object[] { "Hello and welcome to our vibrant community of motorcycle enthusiasts! Whether you're a seasoned rider or just starting out, this is the place to connect, share, and learn from one another.\r\n\r\nHere, you can:\r\n\r\nDiscuss your favorite rides and events 🌍\r\nGet advice on bike builds, maintenance, and repairs 🔧\r\nShare your passion for gear, accessories, and everything in between 🧰\r\nBuy, sell, or trade motorcycles and gear in the Marketplace 🏷️\r\nWe encourage respectful and engaging conversations, so please follow the forum guidelines to ensure a positive experience for everyone.\r\n\r\nWe're excited to have you here! Feel free to introduce yourself, ask questions, and dive into the discussions. Let’s keep the wheels rolling and make this the best community for motorcyclists!\r\n\r\nRide safe,\r\nThe Motosphere Team", new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1906) });

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 10, 30, 58, 872, DateTimeKind.Utc).AddTicks(1886));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegistrationDate" },
                values: new object[] { "f366a811-cf37-4a43-b881-bc750f9368ec", null, new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8572) });

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
                columns: new[] { "Content", "CreatedDate" },
                values: new object[] { "Hello and welcome to our vibrant community of motorcycle enthusiasts! ...", new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8884) });

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 2, 22, 27, 57, 189, DateTimeKind.Utc).AddTicks(8868));
        }
    }
}
