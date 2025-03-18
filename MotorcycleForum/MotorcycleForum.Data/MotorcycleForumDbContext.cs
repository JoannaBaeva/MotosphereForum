using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Web;

namespace MotorcycleForum.Data
{
    public class MotorcycleForumDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MotorcycleForumDbContext(DbContextOptions<MotorcycleForumDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event>? Events { get; set; }
        public DbSet<MarketplaceListing>? MarketplaceListings { get; set; }
        public DbSet<ForumPost>? ForumPosts { get; set; }
        public DbSet<ForumTopic>? ForumTopics { get; set; }
        public DbSet<ForumVote>? ForumVotes { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<EventParticipant>? EventParticipants { get; set; }
        public DbSet<Vote>? Votes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DataSeeding.Seed(modelBuilder);
        }

    }
}
