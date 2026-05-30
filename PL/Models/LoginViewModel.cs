using System.ComponentModel.DataAnnotations;

namespace ASD_Lab3.PL.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Будь ласка, введіть електронну пошту")]
        [EmailAddress(ErrorMessage = "Некоректний формат пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}