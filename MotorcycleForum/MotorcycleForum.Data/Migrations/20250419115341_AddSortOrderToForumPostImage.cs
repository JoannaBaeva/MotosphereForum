using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderToForumPostImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("2763ab56-2f98-4249-bb5f-ff64f8c43a70"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("87c28056-f55e-44ec-b948-834d078389de"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("eb1a2028-fb1d-4d49-a63b-cc5a8288deeb"));

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ForumPostImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2264));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2615));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2595));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2568));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2571));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2573));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("0569735f-5b7b-4300-b090-fc48e99b7cd2"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("41c00239-af2e-4560-854e-77b0de4180e5"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("cc713548-ce4f-46df-b626-56ab7744b65f"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2672));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 19, 11, 53, 40, 732, DateTimeKind.Utc).AddTicks(2667));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("0569735f-5b7b-4300-b090-fc48e99b7cd2"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("41c00239-af2e-4560-854e-77b0de4180e5"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("cc713548-ce4f-46df-b626-56ab7744b65f"));

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ForumPostImages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8315));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8704));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8674));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8637));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8645));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("2763ab56-2f98-4249-bb5f-ff64f8c43a70"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("87c28056-f55e-44ec-b948-834d078389de"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("eb1a2028-fb1d-4d49-a63b-cc5a8288deeb"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8781));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 16, 20, 50, 31, 217, DateTimeKind.Utc).AddTicks(8774));
        }
    }
}
