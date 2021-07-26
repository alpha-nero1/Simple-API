using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PostUser
    {
        public string AppUserId { get; set; }
        public Post Post { get; set; }
        public AppUser AppUser { get; set; }
        public Guid PostId { get; set; }
        public bool IsOwner { get; set; }
    }
}
