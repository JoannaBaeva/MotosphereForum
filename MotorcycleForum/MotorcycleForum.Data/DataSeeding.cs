﻿using System.Globalization;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;

namespace MotorcycleForum.Data
{
    public class DataSeeding
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                    FullName = "MotosphereAdmin",
                    UserName = "motosphere.site@gmail.com",
                    Email = "motosphere.site@gmail.com",
                    EmailConfirmed = true,
                    NormalizedUserName = "MOTOSPHERE.SITE@GMAIL.COM",
                    NormalizedEmail = "MOTOSPHERE.SITE@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEKuWYIdgIxkUUCt5csGiqPiHyIdCSkLWUYZapZJt4A3oHJvIU5ZL/uc7MDB5DXs4Mg==",
                    SecurityStamp = "2f3c8b65-12af-4b6d-bda9-8ec3d8651d3a",
                    ConcurrencyStamp = "e1cb31c4-fcd4-47d7-b1cb-9c7e9edcdd70",
                    Bio = 
                        "I’m the MotosphereAdmin – CEO of \"It Worked on My Machine™\"\r\n😩 I code.\r\n🔫 I moderate.\r\n☕ I run on caffeine, sarcasm, and the tears of broken builds.\r\n\r\nBuilt this place so motorcyclists could post bikes, flex mods, and yell about oil brands in peace.\r\nIf something’s broken, I probably caused it. If it works, you're welcome.\r\nIf you spam, I will smite you with the Ban Hammer™ 🔨🔥\r\n\r\nBasically the final boss of this site. Proceed accordingly.",
                    ProfilePictureUrl = "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/logo.png"
                    // password for admin - X;W0Q6^Ej0Xc 
                },
                new User
                {
                    Id = Guid.Parse("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                    FullName = "JoannaMod",
                    UserName = "joannasofia7@gmail.com",
                    Email = "joannasofia7@gmail.com",
                    EmailConfirmed = true,
                    NormalizedUserName = "JOANNASOFIA7@GMAIL.COM",
                    NormalizedEmail = "JOANNASOFIA7@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEL48ILOb5KeNvfj9rFc1Zaj5+r1ZaA8/gvyxtik5bWH4JZ5us+YaW3nWwSEGdRnxQA==",
                    SecurityStamp = "17dd75f7-5070-4fca-b271-d481b06ada44",
                    ConcurrencyStamp = "e182eb5b-2197-4216-87f3-6be9ba6bddc1",
                    Bio = 
                        "Moderating this place so you don’t have to.\nProbably asleep. Probably judging your post.\nIf it got removed, it deserved it.\nI don't make the rules. Actually, I kinda do.",
                    ProfilePictureUrl = "https://motosphere-images.s3.eu-north-1.amazonaws.com/profiles/cat-profile-pic.jpg"
                    // password - i7;DFK2,aVY1
                }

            );

            // Seed Roles
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = Guid.Parse("711392e6-c020-463d-8a42-01ef90dd6273"), Name = "User", NormalizedName = "USER" },
                new IdentityRole<Guid> { Id = Guid.Parse("39727667-1f7b-488f-8560-1e2942777b94"), Name = "Moderator", NormalizedName = "MODERATOR" },
                new IdentityRole<Guid> { Id = Guid.Parse("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5"), Name = "Admin", NormalizedName = "ADMIN" }
            );

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                    RoleId = Guid.Parse("f15bf949-9c6b-4b98-a6f8-6c4a1c7607b5")
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                    RoleId = Guid.Parse("39727667-1f7b-488f-8560-1e2942777b94")
                }
            );

            //Forum
            // Seed Forum Topics after Users and Roles are inserted
            modelBuilder.Entity<ForumTopic>().HasData(
                new ForumTopic
                {
                    TopicId = 1,
                    Title = "General Discussion"
                },
                new ForumTopic
                {
                    TopicId = 2,
                    Title = "Motorcycle Maintenance & Repair"
                },
                new ForumTopic
                {
                    TopicId = 3,
                    Title = "Gear & Accessories"
                }
            );

            // Seed Forum Posts
            modelBuilder.Entity<ForumPost>().HasData(
                new ForumPost
                {
                    ForumPostId = Guid.Parse("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                    AuthorId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                    Title = "Welcome to the Motosphere Forum! 🏍️",
                    Content = "Hello and welcome to our vibrant community of motorcycle enthusiasts! Whether you're a seasoned rider or just starting out, this is the place to connect, share, and learn from one another.\r\n\r\nHere, you can:\r\n\r\nDiscuss your favorite rides and events 🌍\r\nGet advice on bike builds, maintenance, and repairs 🔧\r\nShare your passion for gear, accessories, and everything in between \U0001f9f0\r\nBuy, sell, or trade motorcycles and gear in the Marketplace 🏷️\r\nWe encourage respectful and engaging conversations, so please follow the forum guidelines to ensure a positive experience for everyone.\r\n\r\nWe're excited to have you here! Feel free to introduce yourself, ask questions, and dive into the discussions. Let’s keep the wheels rolling and make this the best community for motorcyclists!\r\n\r\nRide safe,\r\nThe Motosphere Team",
                    TopicId = 1,
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    CommentId = Guid.Parse("be4ccd71-8576-4378-8b7f-d943f17d19bb"),
                    Content = "<3",
                    AuthorId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                    ForumPostId = Guid.Parse("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"),
                }
            );

            //Marketplace
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = Guid.Parse("d5b06706-b7ed-4252-a257-57b6c4117968"),
                    Name = "Tires & Wheels"
                },
                new Category
                {
                    CategoryId = Guid.Parse("34080d33-7073-48ae-87ee-03c8990ff696"),
                    Name = "Parts & Accessories"
                },
                new Category
                {
                    CategoryId = Guid.Parse("f816c9f1-8132-4a1d-b78d-7370b79500b8"),
                    Name = "Motorcycles for Sale"
                }

            );

            // Seed Marketplace Listings
            modelBuilder.Entity<MarketplaceListing>().HasData(
                new MarketplaceListing(Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"))
                {
                    ListingId = Guid.Parse("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                    Title = "Michelin Road 6 Tires Set",
                    Description = "High-quality road tires for sport-touring motorcycles, excellent grip and durability.",
                    Price = 300.00M,
                    Location = "Varna",
                    CreatedDate = DateTime.SpecifyKind(
                        DateTime.Parse("2025-04-28"),
                        DateTimeKind.Utc
                    ),
                    CategoryId = Guid.Parse("d5b06706-b7ed-4252-a257-57b6c4117968"),
                    IsActive = true,
                    SellerPhoneNumber = "1111111111"
                },
                new MarketplaceListing(Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"))
                {
                    ListingId = Guid.Parse("7998740b-406d-4504-b9df-5f8aef508054"),
                    Title = "Akrapovič Titanium Slip-On Exhaust",
                    Description = "Lightweight, performance-enhancing titanium slip-on exhaust. Fits most sport bikes. Used but in excellent condition. That signature growl? Yeah, it's got it.",
                    Price = 780.00M,
                    Location = "Sofia",
                    CreatedDate = DateTime.SpecifyKind(
                        DateTime.Parse("2025-04-28"),
                        DateTimeKind.Utc
                    ),
                    CategoryId = Guid.Parse("34080d33-7073-48ae-87ee-03c8990ff696"),
                    IsActive = true,
                    SellerPhoneNumber = "1111111111"
                }
            );

            modelBuilder.Entity<MarketplaceListingImage>().HasData(
                new MarketplaceListingImage
                {
                    ImageId = Guid.NewGuid(),
                    ListingId = Guid.Parse("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                    ImageUrl = "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6.png"
                },
                new MarketplaceListingImage
                {
                    ImageId = Guid.NewGuid(),
                    ListingId = Guid.Parse("efcc8a05-65a5-4cb3-859c-ccfc3e6a23bc"),
                    ImageUrl = "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/michelin-tires-6-2.png"
                },
                new MarketplaceListingImage
                {
                    ImageId = Guid.NewGuid(),
                    ListingId = Guid.Parse("7998740b-406d-4504-b9df-5f8aef508054"),
                    ImageUrl = "https://motosphere-images.s3.eu-north-1.amazonaws.com/marketplace/seed-images/Exaust.png"
                }
            );

            modelBuilder.Entity<EventCategory>().HasData(
                new EventCategory
                {
                    CategoryId = Guid.Parse("9eae427f-4376-493b-a662-c1060dc6d30b"),
                    Name = "Motorcycle Show"
                },
                new EventCategory
                {
                    CategoryId = Guid.Parse("85a66af6-084e-46ed-beb4-9b3062b17dc6"),
                    Name = "Workshop"
                }
            );
            
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    EventId = Guid.Parse("6fe266bb-545f-45e3-a66c-ecad09f96171"),
                    Title = "Spring Motorcycle Meetup",
                    Description = "Join us for a big spring motorcycle meetup at the central park!",
                    EventDate = DateTime.SpecifyKind(
                        DateTime.Parse("2025-11-19 13:00"),
                        DateTimeKind.Utc
                    ),
                    Location = "Central Park, NY",
                    CreatedDate = DateTime.SpecifyKind(
                        DateTime.Parse("2025-04-21"),
                        DateTimeKind.Utc
                    ),
                    IsApproved = true,
                    OrganizerId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                    CategoryId = Guid.Parse("9eae427f-4376-493b-a662-c1060dc6d30b")
                },
                new Event
                {
                    EventId = Guid.Parse("bd60185f-e537-4409-8d8a-956f23f0c76a"),
                    Title = "Motorcycle Maintenance Workshop",
                    Description =
                        "Learn the basics of motorcycle maintenance and repair from experienced mechanics!",
                    EventDate = DateTime.SpecifyKind(DateTime.Parse("2025-04-02 11:30"), DateTimeKind.Utc),
                    Location = "Sofia Tech Park",
                    CreatedDate = DateTime.SpecifyKind(DateTime.Parse("2025-03-03"), DateTimeKind.Utc),
                    IsApproved = true,
                    OrganizerId = Guid.Parse("0ab81baf-1cdd-42cd-8d11-391f5118558e"),
                    CategoryId = Guid.Parse("85a66af6-084e-46ed-beb4-9b3062b17dc6")
                }
            );
        }
    }
}
