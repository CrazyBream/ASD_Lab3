using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.DAL.Entities;
using ASD_Lab3.DAL.Interfaces;
using ASD_Lab3.BLL.Infrastructure;

namespace ASD_Lab3.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
        }

        public IEnumerable<CommentDTO> GetCommentsByArticle(int articleId)
        {
            var comments = _database.Comments.Find(c => c.ArticleId == articleId).ToList();
            var dtos = _mapper.Map<IEnumerable<Comment>, List<CommentDTO>>(comments);

            foreach (var dto in dtos)
            {
                var user = _database.Users.Get(dto.UserId);
                if (user != null) dto.AuthorName = user.Username;
            }

            return BuildCommentTree(dtos);
        }

        private List<CommentDTO> BuildCommentTree(List<CommentDTO> allComments)
        {
            var dict = allComments.ToDictionary(c => c.Id);
            var rootComments = new List<CommentDTO>();

            foreach (var c in allComments)
            {
                c.Replies = new List<CommentDTO>();
            }

            foreach (var comment in allComments)
            {
                if (comment.ParentCommentId.HasValue)
                {
                    if (dict.TryGetValue(comment.ParentCommentId.Value, out var parent))
                    {
                        parent.Replies.Add(comment);
                    }
                }
                else
                {
                    rootComments.Add(comment);
                }
            }
            return rootComments;
        }

        public void AddComment(CommentDTO commentDto)
        {
            var comment = new Comment
            {
                Text = commentDto.Text,
                CreatedAt = DateTime.Now,
                UserId = commentDto.UserId,
                ArticleId = commentDto.ArticleId,
                ParentCommentId = commentDto.ParentCommentId
            };

            _database.Comments.Create(comment);
            _database.Save();
        }

        public void UpdateComment(int id, string newText, int userId)
        {
            var comment = _database.Comments.Get(id);
            if (comment == null) throw new ValidationException("Коментар не знайдено", "");
            if (comment.UserId != userId) throw new ValidationException("Немає прав доступу", "");

            comment.Text = newText;

            _database.Comments.Update(comment);
            _database.Save();
        }

        public void DeleteComment(int id, int userId)
        {
            var comment = _database.Comments.Get(id);
            if (comment == null) throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Коментар не знайдено", "");
            if (comment.UserId != userId) throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Немає прав доступу", "");

            DeleteRepliesRecursively(id);

            _database.Comments.Delete(id);
            _database.Save();
        }
        private void DeleteRepliesRecursively(int parentId)
        {
            var replies = _database.Comments.Find(c => c.ParentCommentId == parentId).ToList();

            foreach (var reply in replies)
            {
                DeleteRepliesRecursively(reply.Id);

                _database.Comments.Delete(reply.Id);
            }
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}