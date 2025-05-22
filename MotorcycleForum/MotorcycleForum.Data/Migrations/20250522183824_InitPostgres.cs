using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannedEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    BannedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedEmails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopics",
                columns: table => new
                {
                    TopicId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopics", x => x.TopicId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceListings",
                columns: table => new
                {
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    SellerPhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceListings", x => x.ListingId);
                    table.ForeignKey(
                        name: "FK_MarketplaceListings_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarketplaceListings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    OrganizerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_EventCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumPosts",
                columns: table => new
                {
                    ForumPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    TopicId = table.Column<int>(type: "integer", nullable: true),
                    Upvotes = table.Column<int>(type: "integer", nullable: false),
                    Downvotes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPosts", x => x.ForumPostId);
                    table.ForeignKey(
                        name: "FK_ForumPosts_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ForumPosts_ForumTopics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "ForumTopics",
                        principalColumn: "TopicId");
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceListingImages",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceListingImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_MarketplaceListingImages_MarketplaceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "MarketplaceListings",
                        principalColumn: "ListingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    EventParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => x.EventParticipantId);
                    table.ForeignKey(
                        name: "FK_EventParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForumPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK_Comments_ForumPosts_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPosts",
                        principalColumn: "ForumPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumPostImages",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForumPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPostImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_ForumPostImages_ForumPosts_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPosts",
                        principalColumn: "ForumPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForumPostId = table.Column<Guid>(type: "uuid", nullable: false),
                    VoteType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_ForumPosts_ForumPostId",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPosts",
                        principalColumn: "ForumPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("39727667-1f7b-488f-8560-1e2942777b94"), null, "Moderator", "MODERATOR" },
                    { new Guid("711392e6-c020-463d-8a42-01ef90dd6273"), null, "User", "USER" },
                    { new Guid("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bio", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsBanned", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), 0, "Moderating this place so you don’t have to.\nProbably asleep. Probably judging your post.\nIf it got removed, it deserved it.\nI don't make the rules. Actually, I kinda do.", "e182eb5b-2197-4216-87f3-6be9ba6bddc1", "joannasofia7@gmail.com", true, "JoannaMod", false, false, null, "JOANNASOFIA7@GMAIL.COM", "JOANNASOFIA7@GMAIL.COM", "AQAAAAIAAYagAAAAEL48ILOb5KeNvfj9rFc1Zaj5+r1ZaA8/gvyxtik5bWH4JZ5us+YaW3nWwSEGdRnxQA==", null, false, "https://motosphere-images.s3.eu-north-1.amazonaws.com/profiles/cat-profile-pic.jpg", new DateTime(2025, 5, 22, 18, 38, 23, 795, DateTimeKind.Utc).AddTicks(754), "17dd75f7-5070-4fca-b271-d481b06ada44", false, "joannasofia7@gmail.com" },
                    { new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), 0, "I’m the MotosphereAdmin – CEO of \"It Worked on My Machine™\"\r\n😩 I code.\r\n🔫 I moderate.\r\n☕ I run on caffeine, sarcasm, and the tears of broken builds.\r\n\r\nBuilt this place so motorcyclists could post bikes, flex mods, and yell about oil brands in peace.\r\nIf something’s broken, I probably caused it. If it works, you're welcome.\r\nIf you spam, I will smite you with the Ban Hammer™ 🔨🔥\r\n\r\nBasically the final boss of this site. Proceed accordingly.", "e1cb31c4-fcd4-47d7-b1cb-9c7e9edcdd70", "motosphere.site@gmail.com", true, "MotosphereAdmin", false, false, null, "MOTOSPHERE.SITE@GMAIL.COM", "MOTOSPHERE.SITE@GMAIL.COM", "AQAAAAIAAYagAAAAEKuWYIdgIxkUUCt5csGiqPiHyIdCSkLWUYZapZJt4A3oHJvIU5ZL/uc7MDB5DXs4Mg==", null, false, "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/logo.png", new DateTime(2025, 5, 22, 18, 38, 23, 795, DateTimeKind.Utc).AddTicks(674), "2f3c8b65-12af-4b6d-bda9-8ec3d8651d3a", false, "motosphere.site@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("34080d33-7073-48ae-87ee-03c8990ff696"), "Parts & Accessories" },
                    { new Guid("d5b06706-b7ed-4252-a257-57b6c4117968"), "Tires & Wheels" },
                    { new Guid("f816c9f1-8132-4a1d-b78d-7370b79500b8"), "Motorcycles for Sale" }
                });

            migrationBuilder.InsertData(
                table: "EventCategories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), "Workshop" },
                    { new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), "Motorcycle Show" }
                });

            migrationBuilder.InsertData(
                table: "ForumTopics",
                columns: new[] { "TopicId", "Title" },
                values: new object[,]
                {
                    { 1, "General Discussion" },
                    { 2, "Motorcycle Maintenance & Repair" },
                    { 3, "Gear & Accessories" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("39727667-1f7b-488f-8560-1e2942777b94"), new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e") },
                    { new Guid("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8") }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CategoryId", "CreatedDate", "Description", "EventDate", "IsApproved", "Location", "OrganizerId", "Title" },
                values: new object[,]
                {
                    { new Guid("6fe266bb-545f-45e3-a66c-ecad09f96171"), new Guid("9eae427f-4376-493b-a662-c1060dc6d30b"), new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Join us for a big spring motorcycle meetup at the central park!", new DateTime(2025, 11, 19, 13, 0, 0, 0, DateTimeKind.Unspecified), true, "Central Park, NY", new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Spring Motorcycle Meetup" },
                    { new Guid("bd60185f-e537-4409-8d8a-956f23f0c76a"), new Guid("85a66af6-084e-46ed-beb4-9b3062b17dc6"), new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn the basics of motorcycle maintenance and repair from experienced mechanics!", new DateTime(2025, 4, 2, 11, 30, 0, 0, DateTimeKind.Unspecified), true, "Sofia Tech Park", new Guid("0ab81baf-1cdd-42cd-8d11-391f5118558e"), "Motorcycle Maintenance Workshop" }
                });

            migrationBuilder.InsertData(
                table: "ForumPosts",
                columns: new[] { "ForumPostId", "AuthorId", "Content", "CreatedDate", "Downvotes", "Title", "TopicId", "Upvotes" },
                values: new object[] { new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "Hello and welcome to our vibrant community of motorcycle enthusiasts! Whether you're a seasoned rider or just starting out, this is the place to connect, share, and learn from one another.\r\n\r\nHere, you can:\r\n\r\nDiscuss your favorite rides and events 🌍\r\nGet advice on bike builds, maintenance, and repairs 🔧\r\nShare your passion for gear, accessories, and everything in between 🧰\r\nBuy, sell, or trade motorcycles and gear in the Marketplace 🏷️\r\nWe encourage respectful and engaging conversations, so please follow the forum guidelines to ensure a positive experience for everyone.\r\n\r\nWe're excited to have you here! Feel free to introduce yourself, ask questions, and dive into the discussions. Let’s keep the wheels rolling and make this the best community for motorcyclists!\r\n\r\nRide safe,\r\nThe Motosphere Team", new DateTime(2025, 5, 22, 18, 38, 23, 795, DateTimeKind.Utc).AddTicks(1061), 0, "Welcome to the Motosphere Forum! 🏍️", 1, 0 });

            migrationBuilder.InsertData(
                table: "MarketplaceListings",
                columns: new[] { "ListingId", "CategoryId", "CreatedDate", "Description", "IsActive", "Location", "Price", "SellerId", "SellerPhoneNumber", "Title" },
                values: new object[,]
                {
                    { new Guid("7998740b-406d-4504-b9df-5f8aef508054"), new Guid("34080d33-7073-48ae-87ee-03c8990ff696"), new DateTime(2025, 5, 22, 18, 38, 23, 795, DateTimeKind.Utc).AddTicks(1142), "Lightweight, performance-enhancing titanium slip-on exhaust. Fits most sport bikes. Used but in excellent condition. That signature growl? Yeah, it's got it.", true, "Sofia", 780.00m, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "1111111111", "Akrapovič Titanium Slip-On Exhaust" },
                    { new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"), new Guid("d5b06706-b7ed-4252-a257-57b6c4117968"), new DateTime(2025, 5, 22, 18, 38, 23, 795, DateTimeKind.Utc).AddTicks(1137), "High-quality road tires for sport-touring motorcycles, excellent grip and durability.", true, "Varna", 300.00m, new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "1111111111", "Michelin Road 6 Tires Set" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "AuthorId", "Content", "CreatedDate", "ForumPostId", "ParentCommentId" },
                values: new object[] { new Guid("be4ccd71-8576-4378-8b7f-d943f17d19bb"), new Guid("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), "<3", new DateTime(2025, 5, 22, 18, 38, 23, 795, DateTimeKind.Utc).AddTicks(1082), new Guid("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"), null });

            migrationBuilder.InsertData(
                table: "MarketplaceListingImages",
                columns: new[] { "ImageId", "ImageUrl", "ListingId" },
                values: new object[,]
                {
                    { new Guid("962fc897-86bd-4fdf-9781-ec52e1599c4c"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("dbbc4e30-1373-4878-ac95-0f19078199c8"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png", new Guid("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc") },
                    { new Guid("f864c9e3-bb6d-424e-bc1a-d06f5d83a9c4"), "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png", new Guid("7998740b-406d-4504-b9df-5f8aef508054") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ForumPostId",
                table: "Comments",
                column: "ForumPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventId",
                table: "EventParticipants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_UserId",
                table: "EventParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostImages_ForumPostId",
                table: "ForumPostImages",
                column: "ForumPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPosts_AuthorId",
                table: "ForumPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPosts_TopicId",
                table: "ForumPosts",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListingImages_ListingId",
                table: "MarketplaceListingImages",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListings_CategoryId",
                table: "MarketplaceListings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListings_SellerId",
                table: "MarketplaceListings",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ForumPostId",
                table: "Votes",
                column: "ForumPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BannedEmails");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "ForumPostImages");

            migrationBuilder.DropTable(
                name: "MarketplaceListingImages");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "MarketplaceListings");

            migrationBuilder.DropTable(
                name: "ForumPosts");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ForumTopics");
        }
    }
}
