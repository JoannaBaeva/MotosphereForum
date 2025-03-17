using Humanizer;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Migrations;

namespace MotorcycleForum.Web
{
    public class DataSeeding
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForumTopic>().HasData(
                new ForumTopic { TopicId = 1, 
                    CreatedById = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), 
                    Title = "General Discussion", 
                    IsApproved = bool.Parse("True") }
            );

            modelBuilder.Entity<ForumPost>().HasData(
                new ForumPost { ForumPostId = Guid.Parse("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"), 
                    AuthorId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"),
                    Title = "Welcome to the Motosphere Forum! 🏍️",
                    Content = "Hello and welcome to our vibrant community of motorcycle enthusiasts! Whether you're a seasoned rider or just starting out, this is the place to connect, share, and learn from one another.\r\n\r\nHere, you can:\r\n\r\nDiscuss your favorite rides and events 🌍\r\nGet advice on bike builds, maintenance, and repairs 🔧\r\nShare your passion for gear, accessories, and everything in between \U0001f9f0\r\nBuy, sell, or trade motorcycles and gear in the Marketplace 🏷️\r\nWe encourage respectful and engaging conversations, so please follow the forum guidelines to ensure a positive experience for everyone.\r\n\r\nWe're excited to have you here! Feel free to introduce yourself, ask questions, and dive into the discussions. Let’s keep the wheels rolling and make this the best community for motorcyclists!\r\n\r\nRide safe,\r\nThe Motosphere Team",
                    TopicId = 1,
                }
            );

            modelBuilder.Entity<Comment>().HasData(
            new Comment { CommentId = Guid.Parse("be4ccd71-8576-4378-8b7f-d943f17d19bb"), 
                Content = "<3", 
                AuthorId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8"), 
                ForumPostId = Guid.Parse("c6e5b16e-53f5-41c9-87cd-66da7a096b4a"), 

            }
        );
        }
    }
}
