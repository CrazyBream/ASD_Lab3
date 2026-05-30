using System;
using System.Collections.Generic;

namespace ASD_Lab3.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public User()
        {
            Articles = new List<Article>();
            Comments = new List<Comment>();
        }
    }
}