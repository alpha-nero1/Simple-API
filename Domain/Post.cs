using System;
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
    }
}