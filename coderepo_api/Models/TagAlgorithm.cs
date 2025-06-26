namespace coderepo_api.Models
{
    public class TagAlgorithm
    {
        public char AuditIsDeleted { get; set; }

        public int TagId { get; set; }
        public int AlgorithmId { get; set; }

        public Tag Tag { get; set; } = null!;
        public Algorithm Algorithm { get; set; } = null!;
    }
}
