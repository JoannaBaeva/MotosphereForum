using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Services
{
    public class ForumService
    {
        private readonly MotorcycleForumDbContext _context;

        public ForumService(MotorcycleForumDbContext context)
        {
            _context = context;
        }

    }

}
