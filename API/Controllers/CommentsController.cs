using Microsoft.AspNetCore.Mvc;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.API.Models;
using ASD_Lab3.BLL.Infrastructure;
using System;

namespace ASD_Lab3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("article/{articleId}")]
        public IActionResult GetCommentsByArticle(int articleId) => Ok(_commentService.GetCommentsByArticle(articleId));

        [HttpPost]
        public IActionResult AddComment([FromBody] CommentRequestModel model)
        {
            try
            {
                var commentDto = new CommentDTO
                {
                    Text = model.Text,
                    ArticleId = model.ArticleId,
                    UserId = model.UserId,
                    CreatedAt = DateTime.Now
                };
                _commentService.AddComment(commentDto);
                return Ok(new { message = "Коментар додано!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CommentUpdateRequestModel model)
        {
            try
            {
                _commentService.UpdateComment(id, model.Text, model.UserId);
                return Ok(new { message = "Коментар успішно оновлено!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id, [FromQuery] int userId)
        {
            try
            {
                _commentService.DeleteComment(id, userId);
                return Ok(new { message = "Коментар видалено!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}