using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ForumTopics",
                columns: new[] { "TopicId", "CreatedById", "CreatedDate", "IsApproved", "Title" },
                values: new object[] { 1, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), new DateTime(2025, 3, 17, 9, 25, 1, 798, DateTimeKind.Utc).AddTicks(3473), true, "General Discussion" });

            migrationBuilder.InsertData(
                table: "ForumPosts",
                columns: new[] { "ForumPostId", "AuthorId", "Content", "CreatedDate", "Downvotes", "Title", "TopicId", "Upvotes" },
                values: new object[] { new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Hello and welcome to our vibrant community of motorcycle enthusiasts! Whether you're a seasoned rider or just starting out, this is the place to connect, share, and learn from one another.\r\n\r\nHere, you can:\r\n\r\nDiscuss your favorite rides and events 🌍\r\nGet advice on bike builds, maintenance, and repairs 🔧\r\nShare your passion for gear, accessories, and everything in between 🧰\r\nBuy, sell, or trade motorcycles and gear in the Marketplace 🏷️\r\nWe encourage respectful and engaging conversations, so please follow the forum guidelines to ensure a positive experience for everyone.\r\n\r\nWe're excited to have you here! Feel free to introduce yourself, ask questions, and dive into the discussions. Let’s keep the wheels rolling and make this the best community for motorcyclists!\r\n\r\nRide safe,\r\nThe Motosphere Team", new DateTime(2025, 3, 17, 9, 25, 1, 798, DateTimeKind.Utc).AddTicks(3694), 0, "Welcome to the Motosphere Forum! 🏍️", 1, 0 });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "AuthorId", "CommentId1", "Content", "CreatedDate", "Downvotes", "ForumPostId", "Upvotes" },
                values: new object[] { new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), null, "<3", new DateTime(2025, 3, 17, 9, 25, 1, 798, DateTimeKind.Utc).AddTicks(3755), 0, new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"));

            migrationBuilder.DeleteData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"));

            migrationBuilder.DeleteData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1);
        }
    }
}
