using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedForumTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("202cd740-cde7-4c35-a05b-db980fe0a244"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("2285e3fa-0d42-4ac1-afc4-511752f2cb9c"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("fb4e233f-588b-430a-aa32-c7ce8d93010f"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(52));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 442, DateTimeKind.Utc).AddTicks(9964));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(278));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(233));

            migrationBuilder.InsertData(
                table: "ForumTopics",
                columns: new[] { "TopicId", "CreatedById", "CreatedDate", "IsApproved", "Title" },
                values: new object[,]
                {
                    { 2, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(235), true, "Motorcycle Maintenance & Repair" },
                    { 3, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(237), true, "Gear & Accessories" }
                });

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("33a13eef-03d8-41c9-90c5-5435e69144b4"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("c4e672d6-908c-46ca-bd72-b2b70802c6a1"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("d92fc03d-2cd7-426d-8088-3943b4877ef5"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(329));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(324));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("33a13eef-03d8-41c9-90c5-5435e69144b4"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("c4e672d6-908c-46ca-bd72-b2b70802c6a1"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("d92fc03d-2cd7-426d-8088-3943b4877ef5"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9370));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9289));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9692));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9666));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("202cd740-cde7-4c35-a05b-db980fe0a244"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("2285e3fa-0d42-4ac1-afc4-511752f2cb9c"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("fb4e233f-588b-430a-aa32-c7ce8d93010f"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9844));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 10, 13, 48, 43, 716, DateTimeKind.Utc).AddTicks(9834));
        }
    }
}
