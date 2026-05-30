using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.DAL.Entities;
using ASD_Lab3.DAL.Interfaces;

namespace ASD_Lab3.BLL.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public SearchService(IUnitOfWork database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public IEnumerable<ArticleDTO> GetFilteredArticles(string query, string category)
        {
            var articles = _database.Articles.GetAll("Category", "Author").AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var lowerQuery = query.ToLower();
                articles = articles.Where(a => a.Title.ToLower().Contains(lowerQuery));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var lowerCategory = category.ToLower();
                articles = articles.Where(a => a.Category != null && a.Category.Name.ToLower() == lowerCategory);
            }

            var result = articles.OrderByDescending(a => a.CreatedAt).ToList();
            return _mapper.Map<IEnumerable<Article>, List<ArticleDTO>>(result);
        }

        public IEnumerable<string> GetTopCategories(int count)
        {
            return _database.Articles.GetAll("Category")
                .Where(a => a.Category != null && !string.IsNullOrEmpty(a.Category.Name))
                .GroupBy(a => a.Category.Name)
                .OrderByDescending(g => g.Count())
                .Take(count)
                .Select(g => g.Key)
                .ToList();
        }
    }
}