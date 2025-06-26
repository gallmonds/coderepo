namespace coderepo_api.Models
{
    public class AlgorithmMeta
    {
        public string RootPath { get; set; } = null!;
        public char IsPrivate { get; set; }
        public char IsFlagged { get; set; }
        public char IsDisabled { get; set; }

        public DateTime CreatedAt { get; set; }
        public char AuditIsDeleted { get; set; }
        public int AlgorithmId { get; set; }
        public Algorithm Algorithm { get; set; } = null!;
    }
}
