using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Services.Models.Requests
{
    public class DeleteCommentRequest
    {
        public Guid CommentId { get; set; }
    }
}
