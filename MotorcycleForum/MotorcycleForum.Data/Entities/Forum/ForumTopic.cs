using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Forum
{
    public class ForumTopic
    {
        [Key]
        public int TopicId { get; init; }
        
        [Required]
        public string Title { get; set; } = null!;
        
        [Required]
        public Guid? CreatedById { get; set; }
        
        [Required]
        [ForeignKey(nameof(CreatedById))]
        public User Creator { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;
        public ICollection<ForumPost> Posts { get; set; } = new List<ForumPost>();


    }
}
