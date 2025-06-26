namespace coderepo_api.Models
{
    public class AlgorithmCollaborator
    {
        public char AuditIsDeleted { get; set; }
        public int AlgorithmId { get; set; }
        public int UserId { get; set; }
        public Algorithm Algorithm { get; set; } = null!;
        public DbUser User { get; set; } = null!;
    }
}
