using System.Collections.Generic;
using ASD_Lab3.BLL.DTO;

namespace ASD_Lab3.BLL.Interfaces
{
    public interface ISearchService
    {
        IEnumerable<ArticleDTO> GetFilteredArticles(string query, string category);
        IEnumerable<string> GetTopCategories(int count);
    }
}