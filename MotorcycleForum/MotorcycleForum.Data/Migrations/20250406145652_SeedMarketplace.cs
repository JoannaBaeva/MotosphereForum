using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMarketplace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 6, 14, 56, 51, 105, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("d5b06706-b7ed-4252-a257-57b6c4117968"),
                column: "Name",
                value: "Tires & Wheels");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("34080d33-7073-48ae-87ee-03c8990ff696"), "Parts & Accessories" },
                    { new Guid("f816c9f1-8132-4a1d-b78d-7370b79500b8"), "Motorcycles for Sale" }
                });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 14, 56, 51, 106, DateTimeKind.Utc).AddTicks(149));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 14, 56, 51, 106, DateTimeKind.Utc).AddTicks(124));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 14, 56, 51, 106, DateTimeKind.Utc).AddTicks(102));

            migrationBuilder.InsertData(
                table: "MarketplaceListings",
                columns: new[] { "ListingId", "CategoryId", "CreatedDate", "Description", "IsActive", "Location", "Price", "SellerId", "Title" },
                values: new object[] { new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"), new Guid("d5b06706-b7ed-4252-a257-57b6c4117968"), new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), "High-quality road tires for sport-touring motorcycles, excellent grip and durability.", true, "Varna", 300.00m, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Michelin Road 6 Tires Set" });

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("469abfff-15bf-4a2f-860d-f37f716e5cde"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("d0cd72b5-ba41-4973-8de1-736bd4fce6b4"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("34080d33-7073-48ae-87ee-03c8990ff696"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("f816c9f1-8132-4a1d-b78d-7370b79500b8"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("469abfff-15bf-4a2f-860d-f37f716e5cde"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("d0cd72b5-ba41-4973-8de1-736bd4fce6b4"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 6, 14, 7, 50, 353, DateTimeKind.Utc).AddTicks(5945));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("d5b06706-b7ed-4252-a257-57b6c4117968"),
                column: "Name",
                value: "Tires");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 14, 7, 50, 353, DateTimeKind.Utc).AddTicks(6398));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 14, 7, 50, 353, DateTimeKind.Utc).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 6, 14, 7, 50, 353, DateTimeKind.Utc).AddTicks(6351));
        }
    }
}
