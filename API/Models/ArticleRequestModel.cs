using System.ComponentModel.DataAnnotations;

namespace ASD_Lab3.API.Models
{
    public class ArticleRequestModel
    {
        [Required(ErrorMessage = "Заголовок статті не може бути порожнім!")]
        [MaxLength(100, ErrorMessage = "Заголовок надто довгий (максимум 100 символів).")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Текст статті є обов'язковим!")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Вкажіть рубрику!")]
        public string CategoryName { get; set; }

        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID автора має бути більшим за 0.")]
        public int UserId { get; set; }
    }
}