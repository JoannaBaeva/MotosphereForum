using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Forum
{
    public class ForumPostImage
    {
        [Key]
        public Guid ImageId { get; set; }

        [Required]
        public Guid ForumPostId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}
