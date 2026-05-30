namespace ASD_Lab3.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string? AvatarUrl { get; set; }
    }
}