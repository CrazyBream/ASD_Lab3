using System.ComponentModel.DataAnnotations;

namespace ASD_Lab3.API.Models
{
    public class UserProfileRequestModel
    {
        [Required(ErrorMessage = "Нікнейм є обов'язковим!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Нікнейм має бути від 3 до 50 символів.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Електронна пошта є обов'язковою!")]
        [EmailAddress(ErrorMessage = "Введіть коректну адресу електронної пошти.")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Некоректний формат посилання на аватарку.")]
        public string? AvatarUrl { get; set; }
    }
}