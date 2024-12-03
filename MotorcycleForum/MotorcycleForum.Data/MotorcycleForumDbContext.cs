using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MotorcycleForum.Data
{
    public class MotorcycleForumDbContext : IdentityDbContext<IdentityUser>
    {
        public MotorcycleForumDbContext(DbContextOptions<MotorcycleForumDbContext> options)
            : base(options)
        {
        }

    }
}
