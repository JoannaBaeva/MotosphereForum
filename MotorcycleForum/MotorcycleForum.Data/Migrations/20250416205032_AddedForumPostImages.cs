using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedForumPostImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostImage_ForumPosts_ForumPostId",
                table: "ForumPostImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumPostImage",
                table: "ForumPostImage");

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

            migrationBuilder.RenameTable(
                name: "ForumPostImage",
                newName: "ForumPostImages");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPostImage_ForumPostId",
                table: "ForumPostImages",
                newName: "IX_ForumPostImages_ForumPostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumPostImages",
                table: "ForumPostImages",
                column: "ImageId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostImages_ForumPosts_ForumPostId",
                table: "ForumPostImages",
                column: "ForumPostId",
                principalTable: "ForumPosts",
                principalColumn: "ForumPostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostImages_ForumPosts_ForumPostId",
                table: "ForumPostImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumPostImages",
                table: "ForumPostImages");

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

            migrationBuilder.RenameTable(
                name: "ForumPostImages",
                newName: "ForumPostImage");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPostImages_ForumPostId",
                table: "ForumPostImage",
                newName: "IX_ForumPostImage_ForumPostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumPostImage",
                table: "ForumPostImage",
                column: "ImageId");

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

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(235));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 15, 16, 12, 51, 443, DateTimeKind.Utc).AddTicks(237));

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

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostImage_ForumPosts_ForumPostId",
                table: "ForumPostImage",
                column: "ForumPostId",
                principalTable: "ForumPosts",
                principalColumn: "ForumPostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
