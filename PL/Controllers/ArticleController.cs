using ASD_Lab3.BLL.DTO;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.PL.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace ASD_Lab3.PL.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICommentService _commentService;

        public ArticleController(IArticleService articleService, IWebHostEnvironment webHostEnvironment, ICommentService commentService)
        {
            _articleService = articleService;
            _webHostEnvironment = webHostEnvironment;
            _commentService = commentService;
        }

        public IActionResult Details(int id)
        {
            var article = _articleService.GetAllArticles().FirstOrDefault(a => a.Id == id);
            if (article == null) return NotFound();

            ViewBag.Comments = _commentService.GetCommentsByArticle(id);
            return View(article);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null) return RedirectToAction("Login", "Account");

                string? imageUrl = null;

                if (model.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }

                    imageUrl = "/uploads/" + uniqueFileName;
                }

                var articleDto = new ArticleDTO
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryName = model.CategoryName,
                    ImageUrl = imageUrl,
                    UserId = int.Parse(userIdClaim)
                };

                _articleService.CreateArticle(articleDto);
                return RedirectToAction("Index", "Search");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ArticleCreateViewModel model)
        {
            var article = _articleService.GetAllArticles().FirstOrDefault(a => a.Id == id);
            if (article == null) return NotFound();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || article.UserId.ToString() != userIdClaim) return Forbid();

            string? newImageUrl = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }
                newImageUrl = "/uploads/" + uniqueFileName;
            }

            var updateDto = new ArticleDTO
            {
                Id = id,
                Title = model.Title,
                Content = model.Content,
                CategoryName = model.CategoryName,
                ImageUrl = newImageUrl 
            };

            _articleService.UpdateArticle(updateDto);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var article = _articleService.GetAllArticles().FirstOrDefault(a => a.Id == id);
            if (article == null) return NotFound();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || article.UserId.ToString() != userIdClaim)
            {
                return Forbid();
            }

            _articleService.DeleteArticle(id);
            return RedirectToAction("Index", "Search");
        }

    }
}