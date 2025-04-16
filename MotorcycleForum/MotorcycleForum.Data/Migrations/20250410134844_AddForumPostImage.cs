using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForumPostImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("2aa0029f-baf8-47f7-b641-4fdc4872080e"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("3f16bb78-038c-43b6-b358-b61ba0faf506"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("d08b1a00-5acd-4306-b146-88755ffaa507"));

            migrationBuilder.CreateTable(
                name: "ForumPostImage",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForumPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPostImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_ForumPostImage_ForumPosts_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPosts",
                        principalColumn: "ForumPostId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostImage_ForumPostId",
                table: "ForumPostImage",
                column: "ForumPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPostImage");

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
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(618));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(539));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(867));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(850));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(834));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("2aa0029f-baf8-47f7-b641-4fdc4872080e"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("3f16bb78-038c-43b6-b358-b61ba0faf506"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("d08b1a00-5acd-4306-b146-88755ffaa507"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(916));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 8, 13, 11, 53, 18, DateTimeKind.Utc).AddTicks(912));
        }
    }
}
