using System;

namespace ASD_Lab3.BLL.DTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime RegisteredAt { get; set; }

    }
}