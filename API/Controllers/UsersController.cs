using Microsoft.AspNetCore.Mvc;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.API.Models;
using ASD_Lab3.BLL.Infrastructure;

namespace ASD_Lab3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers() => Ok(_userService.GetAllUsers());

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRequestModel model)
        {
            try
            {
                var registerDto = new UserRegisterDTO
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };
                _userService.Register(registerDto);
                return Ok(new { message = "Користувача успішно зареєстровано!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message, field = ex.Property }); }
        }

        [HttpGet("profile/{userId}")]
        public IActionResult GetProfile(int userId)
        {
            var profile = _userService.GetProfile(userId);
            if (profile == null) return NotFound(new { message = "Профіль не знайдено" });
            return Ok(profile);
        }

        [HttpPut("profile/{userId}")]
        public IActionResult UpdateProfile(int userId, [FromBody] UserProfileRequestModel model)
        {
            try
            {
                var profileDto = new UserProfileDTO
                {
                    Id = userId,
                    Username = model.Username,
                    Email = model.Email,
                    AvatarUrl = model.AvatarUrl
                };
                _userService.UpdateProfile(profileDto);
                return Ok(new { message = "Профіль успішно оновлено!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message, field = ex.Property }); }
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteAccount(int userId)
        {
            try
            {
                _userService.DeleteAccount(userId);
                return Ok(new { message = "Акаунт успішно видалено з системи!" });
            }
            catch (ValidationException ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}