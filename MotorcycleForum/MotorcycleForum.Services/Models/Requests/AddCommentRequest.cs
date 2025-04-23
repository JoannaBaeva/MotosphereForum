using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Services.Models.Requests
{
    public class AddCommentRequest
    {
        public Guid PostId { get; set; }
        public string? Content { get; set; }
    }
}
