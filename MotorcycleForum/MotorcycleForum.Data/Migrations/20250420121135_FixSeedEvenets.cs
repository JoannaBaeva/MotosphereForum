using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedEvenets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("0c31488a-8864-4b6e-8680-d756f3348db4"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("c5ce8a8c-eba0-43b2-a747-a07440ccf7b0"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("36e6b95a-2a40-4fea-b866-82faf3ed2c8c"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("78bd8d6a-e5b0-40ee-a953-2eed61cf21f7"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("b2b16c2b-f2ae-4492-a24d-1ca42f5f2b96"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7644));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7975));

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "CreatedDate", "Description", "EventDate", "IsApproved", "Location", "OrganizerId", "Title" },
                values: new object[,]
                {
                    { new Guid("4049943b-1c0f-47c1-9cbe-3086129a4fd5"), new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), new DateTime(2025, 4, 25, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8126), "Join us for a big spring motorcycle meetup at the central park!", new DateTime(2025, 5, 22, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8116), true, "Central Park, NY", new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Spring Motorcycle Meetup" },
                    { new Guid("aaaff0b2-f3ca-47a2-9f6f-1beb82562b1e"), new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), new DateTime(2025, 3, 16, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8131), "Learn the basics of motorcycle maintenance and repair from experienced mechanics!", new DateTime(2025, 3, 31, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8130), true, "Sofia Tech Park", new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), "Motorcycle Maintenance Workshop" }
                });

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7953));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7892));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7894));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7897));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("05e95a5e-47fa-41ee-83bf-cea1a109a14f"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("54a4edbf-3b05-4270-b267-787ba43b2329"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("54f61b31-0412-40a2-aad0-a3973bd6c353"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8036));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8032));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("4049943b-1c0f-47c1-9cbe-3086129a4fd5"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("aaaff0b2-f3ca-47a2-9f6f-1beb82562b1e"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("05e95a5e-47fa-41ee-83bf-cea1a109a14f"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("54a4edbf-3b05-4270-b267-787ba43b2329"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("54f61b31-0412-40a2-aad0-a3973bd6c353"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(1713));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(1572));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2078));

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "CreatedDate", "Description", "EventDate", "IsApproved", "Location", "OrganizerId", "Title" },
                values: new object[,]
                {
                    { new Guid("0c31488a-8864-4b6e-8680-d756f3348db4"), new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), new DateTime(2025, 4, 15, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2233), "Learn the basics of motorcycle maintenance and repair from experienced mechanics!", new DateTime(2025, 5, 10, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2232), true, "Sofia Tech Park", new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), "Motorcycle Maintenance Workshop" },
                    { new Guid("c5ce8a8c-eba0-43b2-a747-a07440ccf7b0"), new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), new DateTime(2025, 4, 28, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2228), "Join us for a big spring motorcycle meetup at the central park!", new DateTime(2025, 4, 30, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2217), true, "Central Park, NY", new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Spring Motorcycle Meetup" }
                });

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2058));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2026));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2029));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2031));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("36e6b95a-2a40-4fea-b866-82faf3ed2c8c"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("78bd8d6a-e5b0-40ee-a953-2eed61cf21f7"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("b2b16c2b-f2ae-4492-a24d-1ca42f5f2b96"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2143));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 8, 58, 278, DateTimeKind.Utc).AddTicks(2138));
        }
    }
}
