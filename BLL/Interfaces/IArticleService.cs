using System.Collections.Generic;
using ASD_Lab3.BLL.DTO;

namespace ASD_Lab3.BLL.Interfaces
{
    public interface IArticleService
    {
        IEnumerable<ArticleDTO> GetAllArticles();
        ArticleDTO GetArticle(int id);
        void CreateArticle(ArticleDTO articleDto);
        void UpdateArticle(ArticleDTO articleDto);
        void DeleteArticle(int id);
        void Dispose();
    }
}