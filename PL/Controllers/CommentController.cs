using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.BLL.Interfaces;
using System.Security.Claims;

namespace ASD_Lab3.PL.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int articleId, string text, int? parentCommentId)
        {
            if (string.IsNullOrWhiteSpace(text)) return RedirectToAction("Details", "Article", new { id = articleId });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var dto = new CommentDTO
            {
                ArticleId = articleId,
                Text = text,
                ParentCommentId = parentCommentId,
                UserId = userId
            };

            _commentService.AddComment(dto);
            return RedirectToAction("Details", "Article", new { id = articleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int articleId, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return RedirectToAction("Details", "Article", new { id = articleId });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _commentService.UpdateComment(id, text, userId);

            return RedirectToAction("Details", "Article", new { id = articleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int articleId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _commentService.DeleteComment(id, userId);
            return RedirectToAction("Details", "Article", new { id = articleId });
        }
    }
}