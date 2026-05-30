using System.ComponentModel.DataAnnotations;

namespace ASD_Lab3.PL.Models
{
    public class ArticleCreateViewModel
    {
        [Required(ErrorMessage = "Придумайте крутий заголовок!")]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Стаття не може бути порожньою")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Вкажіть рубрику")]
        public string CategoryName { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}