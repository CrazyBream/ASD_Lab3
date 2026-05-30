using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.BLL.Infrastructure;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.DAL.Entities;
using ASD_Lab3.DAL.Interfaces;

namespace ASD_Lab3.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
        }

        public IEnumerable<ArticleDTO> GetAllArticles()
        {
            var articles = _database.Articles.GetAll().ToList();
            var dtos = _mapper.Map<IEnumerable<Article>, List<ArticleDTO>>(articles);

            foreach (var dto in dtos)
            {
                var category = _database.Categories.Get(dto.CategoryId);
                if (category != null) dto.CategoryName = category.Name;

                var user = _database.Users.Get(dto.UserId);
                if (user != null) dto.AuthorName = user.Username;
            }

            return dtos;
        }
        public ArticleDTO GetArticle(int id)
        {
            var article = _database.Articles.Get(id);
            if (article == null)
                throw new ValidationException("Статтю не знайдено", "");

            return _mapper.Map<Article, ArticleDTO>(article);
        }

        public void CreateArticle(ArticleDTO articleDto)
        {
            string catName = string.IsNullOrWhiteSpace(articleDto.CategoryName) ? "Загальне" : articleDto.CategoryName.Trim();
            var category = _database.Categories.Find(c => c.Name.ToLower() == catName.ToLower()).FirstOrDefault();

            if (category == null)
            {
                category = new Category { Name = catName };
                _database.Categories.Create(category);
                _database.Save();
            }

            var article = new Article
            {
                Title = articleDto.Title,
                Content = articleDto.Content,
                UserId = articleDto.UserId,
                CategoryId = category.Id,
                ImageUrl = articleDto.ImageUrl,
                CreatedAt = DateTime.Now 
            };

            _database.Articles.Create(article);
            _database.Save();
        }

        public void UpdateArticle(ArticleDTO articleDto)
        {
            var article = _database.Articles.Get(articleDto.Id);
            if (article == null) throw new ValidationException("Статтю не знайдено", "");

            string catName = string.IsNullOrWhiteSpace(articleDto.CategoryName) ? "Загальне" : articleDto.CategoryName.Trim();
            var category = _database.Categories.Find(c => c.Name.ToLower() == catName.ToLower()).FirstOrDefault();

            if (category == null)
            {
                category = new Category { Name = catName };
                _database.Categories.Create(category);
                _database.Save();
            }

            article.Title = articleDto.Title;
            article.Content = articleDto.Content;
            article.CategoryId = category.Id;

            if (!string.IsNullOrEmpty(articleDto.ImageUrl))
            {
                article.ImageUrl = articleDto.ImageUrl;
            }

            _database.Articles.Update(article);
            _database.Save();
        }

        public void DeleteArticle(int id)
        {
            var article = _database.Articles.Get(id);
            if (article == null)
                throw new ValidationException("Статтю не знайдено", "");

            _database.Articles.Delete(id);
            _database.Save();
        }
        public void Dispose()
        {
            _database.Dispose();
        }
    }
}