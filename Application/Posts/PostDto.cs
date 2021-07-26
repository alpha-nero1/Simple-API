using Application.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Topic { get; set; }
        public string OwnerUsername { get; set; }
        // Allows for soft deletion of the model.
        public DateTime? DateDisabled { get; set; }
        // Profiles are a 'DTO' for post users.
        public ICollection<Profile> PostUsers { get; set; }
    }
}
