using System.Collections.Generic;
using ASD_Lab3.BLL.DTO;

namespace ASD_Lab3.BLL.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentDTO> GetCommentsByArticle(int articleId);
        void AddComment(CommentDTO commentDto);
        void DeleteComment(int id, int userId);
        void UpdateComment(int id, string newText, int userId);

        void Dispose();
    }
}