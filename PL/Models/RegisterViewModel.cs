using System.ComponentModel.DataAnnotations;

namespace ASD_Lab3.PL.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Логін є обов'язковим")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логін має бути від 3 до 50 символів")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Введіть коректний Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Мінімум 6 символів")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}