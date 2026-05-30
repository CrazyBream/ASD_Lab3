using System;
using System.Collections.Generic;

namespace ASD_Lab3.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string AuthorName { get; set; }
        public int ArticleId { get; set; }
        public int? ParentCommentId { get; set; }
        public List<CommentDTO> Replies { get; set; } = new List<CommentDTO>();
    }
}