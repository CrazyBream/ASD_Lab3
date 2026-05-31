using Microsoft.AspNetCore.Mvc;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.API.Models; // Твій namespace моделей
using ASD_Lab3.BLL.Infrastructure; // Для ValidationException

namespace ASD_Lab3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult GetAllArticles() => Ok(_articleService.GetAllArticles());

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            try
            {
                var article = _articleService.GetArticle(id);
                return Ok(article);
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost]
        public IActionResult CreateArticle([FromBody] ArticleRequestModel model)
        {
            try
            {
                var articleDto = new ArticleDTO
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryName = model.CategoryName,
                    ImageUrl = model.ImageUrl,
                    UserId = model.UserId
                };
                _articleService.CreateArticle(articleDto);
                return Ok(new { message = "Статтю успішно створено!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateArticle(int id, [FromBody] ArticleRequestModel model)
        {
            try
            {
                var updateDto = new ArticleDTO
                {
                    Id = id,
                    Title = model.Title,
                    Content = model.Content,
                    CategoryName = model.CategoryName,
                    ImageUrl = model.ImageUrl
                };
                _articleService.UpdateArticle(updateDto);
                return Ok(new { message = "Статтю успішно оновлено!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(int id)
        {
            try
            {
                _articleService.DeleteArticle(id);
                return Ok(new { message = "Статтю видалено!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}