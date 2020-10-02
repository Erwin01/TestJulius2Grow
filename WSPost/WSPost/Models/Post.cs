using System;
using System.ComponentModel.DataAnnotations;

namespace WSPost.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImgPath { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
    }
}
