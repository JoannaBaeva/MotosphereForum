using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededEventsIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumVotes");

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("4c027f93-0d70-4e06-9b38-1fa7c640ee17"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("507c4436-d3dd-4acf-9f7e-e57c2b5ca602"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("622f9b25-cc5f-4084-9575-6235679f2f97"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("d327c5a0-fdad-483f-a328-aab64546ed40"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("fee5a5df-cd09-45ec-b51f-ad08ca1cfbe9"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 21, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(5775));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 21, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(5690));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(5996));

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "CreatedDate", "Description", "EventDate", "IsApproved", "Location", "OrganizerId", "Title" },
                values: new object[,]
                {
                    { new Guid("6fe266bb-545f-45e3-a66c-ecad09f96171"), new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), new DateTime(2025, 4, 26, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(6130), "Join us for a big spring motorcycle meetup at the central park!", new DateTime(2025, 5, 23, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(6122), true, "Central Park, NY", new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Spring Motorcycle Meetup" },
                    { new Guid("bd60185f-e537-4409-8d8a-956f23f0c76a"), new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), new DateTime(2025, 3, 17, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(6134), "Learn the basics of motorcycle maintenance and repair from experienced mechanics!", new DateTime(2025, 4, 1, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(6133), true, "Sofia Tech Park", new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), "Motorcycle Maintenance Workshop" }
                });

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(5974));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("1d8ea6bb-7c81-4d64-a0d1-fa83d0e9edca"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("7e58ec6f-7926-40d4-9531-e9f8b56c2494"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("ad6546c2-6be0-4307-852d-a182109ec3d1"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(6054));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 12, 0, 36, 648, DateTimeKind.Utc).AddTicks(6048));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("6fe266bb-545f-45e3-a66c-ecad09f96171"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("bd60185f-e537-4409-8d8a-956f23f0c76a"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("1d8ea6bb-7c81-4d64-a0d1-fa83d0e9edca"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("7e58ec6f-7926-40d4-9531-e9f8b56c2494"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("ad6546c2-6be0-4307-852d-a182109ec3d1"));

            migrationBuilder.CreateTable(
                name: "ForumVotes",
                columns: table => new
                {
                    VoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoteType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumVotes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_ForumVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ForumVotes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK_ForumVotes_ForumPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "ForumPosts",
                        principalColumn: "ForumPostId");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(2776));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(2646));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3026));

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "CreatedDate", "Description", "EventDate", "IsApproved", "Location", "OrganizerId", "Title" },
                values: new object[,]
                {
                    { new Guid("4c027f93-0d70-4e06-9b38-1fa7c640ee17"), new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), new DateTime(2025, 4, 26, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3218), "Join us for a big spring motorcycle meetup at the central park!", new DateTime(2025, 5, 23, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3209), true, "Central Park, NY", new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Spring Motorcycle Meetup" },
                    { new Guid("507c4436-d3dd-4acf-9f7e-e57c2b5ca602"), new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), new DateTime(2025, 3, 17, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3223), "Learn the basics of motorcycle maintenance and repair from experienced mechanics!", new DateTime(2025, 4, 1, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3222), true, "Sofia Tech Park", new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), "Motorcycle Maintenance Workshop" }
                });

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3001));

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("622f9b25-cc5f-4084-9575-6235679f2f97"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("d327c5a0-fdad-483f-a328-aab64546ed40"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("fee5a5df-cd09-45ec-b51f-ad08ca1cfbe9"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3142));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.CreateIndex(
                name: "IX_ForumVotes_CommentId",
                table: "ForumVotes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumVotes_PostId",
                table: "ForumVotes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumVotes_UserId",
                table: "ForumVotes",
                column: "UserId");
        }
    }
}
