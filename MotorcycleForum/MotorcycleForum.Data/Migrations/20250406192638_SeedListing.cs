using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("7cf612ea-5400-4bef-9af9-a7f4291433fe"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("fc90e9f8-8f1f-464e-9883-dc3d0297e288"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(472));

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
                    { new Guid("c8740c2c-470b-4b86-bbf8-59f1f9374ade"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(869));

            migrationBuilder.InsertData(
                table: "MarketplaceListings",
                columns: new[] { "ListingId", "CategoryId", "CreatedDate", "Description", "IsActive", "Location", "Price", "SellerId", "Title" },
                values: new object[] { new Guid("7998740b-406d-4504-b9df-5f8aef508054"), new Guid("34080d33-7073-48ae-87ee-03c8990ff696"), new DateTime(2025, 4, 6, 19, 26, 37, 415, DateTimeKind.Utc).AddTicks(873), "Lightweight, performance-enhancing titanium slip-on exhaust. Fits most sport bikes. Used but in excellent condition. That signature growl? Yeah, it's got it.", true, "Sofia", 780.00m, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Akrapovič Titanium Slip-On Exhaust" });

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[] { new Guid("e327d48a-0898-4678-9a75-acad93204464"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 6, 18, 21, 21, 942, DateTimeKind.Utc).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 18, 21, 21, 942, DateTimeKind.Utc).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 18, 21, 21, 942, DateTimeKind.Utc).AddTicks(5305));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 18, 21, 21, 942, DateTimeKind.Utc).AddTicks(5290));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("7cf612ea-5400-4bef-9af9-a7f4291433fe"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("fc90e9f8-8f1f-464e-9883-dc3d0297e288"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 18, 21, 21, 942, DateTimeKind.Utc).AddTicks(5408));
        }
    }
}
