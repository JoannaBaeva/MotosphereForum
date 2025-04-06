using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketplaceListingImages_MarketplaceListings_ListingId",
                table: "MarketplaceListingImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "ListingId",
                table: "MarketplaceListingImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 4, 14, 1, 42, 751, DateTimeKind.Utc).AddTicks(3344));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 14, 1, 42, 751, DateTimeKind.Utc).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 14, 1, 42, 751, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 14, 1, 42, 751, DateTimeKind.Utc).AddTicks(3701));

            migrationBuilder.AddForeignKey(
                name: "FK_MarketplaceListingImages_MarketplaceListings_ListingId",
                table: "MarketplaceListingImages",
                column: "ListingId",
                principalTable: "MarketplaceListings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketplaceListingImages_MarketplaceListings_ListingId",
                table: "MarketplaceListingImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "ListingId",
                table: "MarketplaceListingImages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 4, 13, 59, 25, 594, DateTimeKind.Utc).AddTicks(887));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 13, 59, 25, 594, DateTimeKind.Utc).AddTicks(1278));

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 13, 59, 25, 594, DateTimeKind.Utc).AddTicks(1261));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 4, 4, 13, 59, 25, 594, DateTimeKind.Utc).AddTicks(1241));

            migrationBuilder.AddForeignKey(
                name: "FK_MarketplaceListingImages_MarketplaceListings_ListingId",
                table: "MarketplaceListingImages",
                column: "ListingId",
                principalTable: "MarketplaceListings",
                principalColumn: "ListingId");
        }
    }
}
