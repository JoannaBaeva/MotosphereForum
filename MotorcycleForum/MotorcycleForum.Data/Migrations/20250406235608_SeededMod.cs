using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("5b6a314d-d38e-4709-ad7c-72f6cdfcc19a"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("c8740c2c-470b-4b86-bbf8-59f1f9374ade"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("e327d48a-0898-4678-9a75-acad93204464"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ProfilePictureUrl", "RegistrationDate" },
                values: new object[] { "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/logo.png", new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(7671) });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), 0, null, "ebc70cd0-b046-47f8-aa13-eefefff8494b", "joannasofia7@gmail.com", true, "JoannaMod", false, null, "JOANNASOFIA7@GMAIL.COM", "JOANNASOFIA7@GMAIL.COM", "AQAAAAIAAYagAAAAEL48ILOb5KeNvfj9rFc1Zaj5+r1ZaA8/gvyxtik5bWH4JZ5us+YaW3nWwSEGdRnxQA==", null, false, "https://motosphere-images.s3.eu-north-1.amazonaws.com/profiles/cat-profile-pic.jpg", new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(7748), null, false, "joannasofia7@gmail.com" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(8040));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(7989));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(7959));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("17fdc64f-a3e4-4e6e-8464-c1abc729e6e3"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("51fd048a-e101-4e2d-ac09-95c1224d45d3"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("90b15b59-383c-4b2a-bf8f-e058e7da9ee7"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(8091));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 23, 56, 8, 179, DateTimeKind.Utc).AddTicks(8084));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("39727667-1f7b-488f-8560-1e2942777b94"), new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("39727667-1f7b-488f-8560-1e2942777b94"), new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e") });

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("17fdc64f-a3e4-4e6e-8464-c1abc729e6e3"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("51fd048a-e101-4e2d-ac09-95c1224d45d3"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("90b15b59-383c-4b2a-bf8f-e058e7da9ee7"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "ProfilePictureUrl", "RegistrationDate" },
                values: new object[] { null, new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(472) });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(826));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(807));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(789));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("5b6a314d-d38e-4709-ad7c-72f6cdfcc19a"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("c8740c2c-470b-4b86-bbf8-59f1f9374ade"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("e327d48a-0898-4678-9a75-acad93204464"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(873));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(869));
        }
    }
}
