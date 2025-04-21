using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class ForumTopicEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatedById",
                table: "ForumTopics");

            migrationBuilder.DropIndex(
                name: "IX_ForumTopics_CreatedById",
                table: "ForumTopics");

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("4049943b-1c0f-47c1-9cbe-3086129a4fd5"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("aaaff0b2-f3ca-47a2-9f6f-1beb82562b1e"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("05e95a5e-47fa-41ee-83bf-cea1a109a14f"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("54a4edbf-3b05-4270-b267-787ba43b2329"));

            migrationBuilder.DeleteData(
                table: "MarketplaceListingImages",
                keyColumn: "ImageId",
                keyValue: new Guid("54f61b31-0412-40a2-aad0-a3973bd6c353"));

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ForumTopics");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ForumTopics");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                columns: new[] { "IsBanned", "RegistrationDate" },
                values: new object[] { false, new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(2776) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                columns: new[] { "IsBanned", "RegistrationDate" },
                values: new object[] { false, new DateTime(2025, 4, 21, 11, 3, 49, 427, DateTimeKind.Utc).AddTicks(2646) });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedEmails");

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

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ForumTopics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ForumTopics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ForumTopics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7644));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                column: "RegistrationDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7975));

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "CreatedDate", "Description", "EventDate", "IsApproved", "Location", "OrganizerId", "Title" },
                values: new object[,]
                {
                    { new Guid("4049943b-1c0f-47c1-9cbe-3086129a4fd5"), new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), new DateTime(2025, 4, 25, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8126), "Join us for a big spring motorcycle meetup at the central park!", new DateTime(2025, 5, 22, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8116), true, "Central Park, NY", new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Spring Motorcycle Meetup" },
                    { new Guid("aaaff0b2-f3ca-47a2-9f6f-1beb82562b1e"), new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), new DateTime(2025, 3, 16, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8131), "Learn the basics of motorcycle maintenance and repair from experienced mechanics!", new DateTime(2025, 3, 31, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8130), true, "Sofia Tech Park", new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), "Motorcycle Maintenance Workshop" }
                });

            migrationBuilder.UpdateData(
                table: "ForumPosts",
                keyColumn: "ForumPostId",
                keyValue: new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7953));

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedDate", "IsApproved" },
                values: new object[] { new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7892), true });

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 2,
                columns: new[] { "CreatedById", "CreatedDate", "IsApproved" },
                values: new object[] { new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7894), true });

            migrationBuilder.UpdateData(
                table: "ForumTopics",
                keyColumn: "TopicId",
                keyValue: 3,
                columns: new[] { "CreatedById", "CreatedDate", "IsApproved" },
                values: new object[] { new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(7897), true });

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("05e95a5e-47fa-41ee-83bf-cea1a109a14f"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("54a4edbf-3b05-4270-b267-787ba43b2329"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") },
                    { new Guid("54f61b31-0412-40a2-aad0-a3973bd6c353"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") }
                });

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("7998740b-406d-4504-b9df-5f8aef508054"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8036));

            migrationBuilder.UpdateData(
                table: "MarketplaceListings",
                keyColumn: "ListingId",
                keyValue: new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                column: "CreatedDate",
                value: new DateTime(2025, 4, 20, 12, 11, 34, 475, DateTimeKind.Utc).AddTicks(8032));

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_CreatedById",
                table: "ForumTopics",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatedById",
                table: "ForumTopics",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
