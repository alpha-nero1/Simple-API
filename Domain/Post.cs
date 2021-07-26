using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Topic { get; set; }
        public DateTime? DateDisabled { get; set; }
        // Ensures we do not get a null ref when trying to add a post user to post.
        public ICollection<PostUser> PostUsers = new List<PostUser>();
    }
}