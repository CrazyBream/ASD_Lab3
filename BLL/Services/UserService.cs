using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ASD_Lab3.BLL.DTO;
using ASD_Lab3.BLL.Infrastructure;
using ASD_Lab3.BLL.Interfaces;
using ASD_Lab3.DAL.Entities;
using ASD_Lab3.DAL.Interfaces;

namespace ASD_Lab3.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow;
            _mapper = mapper;
        }

        public IEnumerable<UserDTO> GetAllUsers() =>
            _mapper.Map<IEnumerable<User>, List<UserDTO>>(_database.Users.GetAll().ToList());

        public UserDTO GetUser(int id)
        {
            var user = _database.Users.Get(id);
            if (user == null) throw new ValidationException("Користувача не знайдено", "");
            return _mapper.Map<User, UserDTO>(user);
        }

        public void Register(UserRegisterDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Email))
                throw new ValidationException("Усі поля є обов'язковими для заповнення", "");

            var existingUser = _database.Users.Find(u => u.Email == dto.Email).FirstOrDefault();
            if (existingUser != null)
                throw new ValidationException("Користувач з такою електронною поштою вже існує", "Email");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                RegisteredAt = DateTime.Now 
            };

            _database.Users.Create(user);
            _database.Save();
        }

        public UserDTO Authenticate(UserLoginDTO dto)
        {
            var hash = HashPassword(dto.Password);

            var user = _database.Users.Find(u => u.Email == dto.Email && u.PasswordHash == hash).FirstOrDefault();

            if (user == null)
                throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Неправильна пошта або пароль", "");

            return _mapper.Map<User, UserDTO>(user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
        public UserProfileDTO GetProfile(int userId)
        {
            var user = _database.Users.Get(userId);
            if (user == null) return null;

            return new UserProfileDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Bio = user.Bio,
                RegisteredAt = user.RegisteredAt
            };
        }

        public void UpdateProfile(UserProfileDTO profileDto)
        {
            var user = _database.Users.Get(profileDto.Id);
            if (user == null) throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Користувача не знайдено", "");

            if (user.Username != profileDto.Username)
            {
                var existingUser = _database.Users.Find(u => u.Username == profileDto.Username).FirstOrDefault();
                if (existingUser != null)
                    throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Користувач з таким ім'ям вже існує!", "Username");

                user.Username = profileDto.Username;
            }

            user.Bio = profileDto.Bio;
            if (!string.IsNullOrEmpty(profileDto.AvatarUrl))
            {
                user.AvatarUrl = profileDto.AvatarUrl;
            }

            _database.Users.Update(user);
            _database.Save();
        }

        public void DeleteAccount(int userId)
        {
            var comments = _database.Comments.Find(c => c.UserId == userId).ToList();
            foreach (var c in comments) _database.Comments.Delete(c.Id);

            var articles = _database.Articles.Find(a => a.UserId == userId).ToList();
            foreach (var a in articles) _database.Articles.Delete(a.Id);

            _database.Users.Delete(userId);
            _database.Save();
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _database.Users.Get(userId);
            if (user == null) throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Користувача не знайдено", "");

            if (user.PasswordHash != HashPassword(oldPassword))
                throw new ASD_Lab3.BLL.Infrastructure.ValidationException("Неправильний старий пароль", "");

            user.PasswordHash = HashPassword(newPassword);

            _database.Users.Update(user);
            _database.Save();
        }

        public void Dispose() => _database.Dispose();
    }
}