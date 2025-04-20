using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Event_Tracker;

namespace MotorcycleForum.Data
{
    public class MotorcycleForumDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MotorcycleForumDbContext(DbContextOptions<MotorcycleForumDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<MarketplaceListing> MarketplaceListings { get; set; } = null!;
        public DbSet<MarketplaceListingImage> MarketplaceListingImages { get; set; } = null!;
        public DbSet<ForumPost> ForumPosts { get; set; } = null!;
        public DbSet<ForumPostImage> ForumPostImages { get; set; } = null!;
        public DbSet<ForumTopic> ForumTopics { get; set; } = null!;
        public DbSet<ForumVote> ForumVotes { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<EventParticipant> EventParticipants { get; set; } = null!;
        public DbSet<EventCategory> EventCategories { get; set; } = null!;
        public DbSet<Vote> Votes { get; set; } = null!; 



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DataSeeding.Seed(modelBuilder);

        }

    }
}
