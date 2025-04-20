using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("1f6ac685-2310-41cc-94a4-7b09e7503819"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("99bc46b1-e402-4484-a56e-579084274cad"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("c68e65fa-18ac-400e-9aa7-fbd5bd2cad9b"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "Events",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8194));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8106));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8452));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8423));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8428));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("1f6ac685-2310-41cc-94a4-7b09e7503819"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("99bc46b1-e402-4484-a56e-579084274cad"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("c68e65fa-18ac-400e-9aa7-fbd5bd2cad9b"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8532));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 10, 27, 43, 545, DateTimeKind.Utc).AddTicks(8527));
        }
    }
}
