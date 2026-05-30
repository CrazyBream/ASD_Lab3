using System;
using System.Collections.Generic;

namespace ASD_Lab3.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

        public int UserId { get; set; }
        public virtual User Author { get; set; }

        public int? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }

        public Comment()
        {
            Replies = new List<Comment>();
        }
    }
}