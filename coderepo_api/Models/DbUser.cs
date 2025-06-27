namespace coderepo_api.Models
{
    public class DbUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Biography { get; set; } = string.Empty;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public char IsFlagged { get; set; }
        public char IsBanned { get; set; }
        public char auditIsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PfpId { get; set; }

        public Media MediaDb { get; set; } = null!;

        public List<Algorithm> Algorithms { get; set; } = new List<Algorithm>();
        public List<AlgorithmCollaborator> AlgorithmCollaborators { get; set; } = new List<AlgorithmCollaborator>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
