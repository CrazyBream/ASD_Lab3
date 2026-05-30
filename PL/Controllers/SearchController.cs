using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ASD_Lab3.BLL.Interfaces;

namespace ASD_Lab3.PL.Controllers
{
    [AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IActionResult Index(string query, string category)
        {
            var articles = _searchService.GetFilteredArticles(query, category);

            ViewBag.TopCategories = _searchService.GetTopCategories(5);
            ViewBag.CurrentQuery = query;
            ViewBag.CurrentCategory = category;

            return View(articles);
        }
    }
}