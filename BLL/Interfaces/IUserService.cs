using System.Collections.Generic;
using ASD_Lab3.BLL.DTO;

namespace ASD_Lab3.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUser(int id);
        void Register(UserRegisterDTO dto); 
        UserDTO Authenticate(UserLoginDTO dto);
        UserProfileDTO GetProfile(int userId);
        void UpdateProfile(UserProfileDTO profileDto);
        void ChangePassword(int userId, string oldPassword, string newPassword);
        void DeleteAccount(int userId);
        void Dispose();
    }
}