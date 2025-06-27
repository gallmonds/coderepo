namespace coderepo_api.Dtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ProfilePic { get; set; } = string.Empty;
    }
}
