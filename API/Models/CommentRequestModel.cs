using System.ComponentModel.DataAnnotations;

namespace ASD_Lab3.API.Models
{
    public class CommentRequestModel
    {
        [Required(ErrorMessage = "Текст коментаря є обов'язковим!")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Коментар має містити від 2 до 1000 символів.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "ID статті є обов'язковим.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID статті має бути більшим за 0.")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "ID користувача є обов'язковим.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID користувача має бути більшим за 0.")]
        public int UserId { get; set; }
    }
}