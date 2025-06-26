namespace coderepo_api.Models
{
    public class Rating
    {
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public int ContentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public char AuditIsDeleted { get; set; }
        public int Version { get; set; }

        public DbUser User { get; set; } = null!;
        public Algorithm Algorithm { get; set; } = null!;
    }
}
