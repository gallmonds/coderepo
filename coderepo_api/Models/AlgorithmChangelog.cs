namespace coderepo_api.Models
{
    public class AlgorithmChangelog
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public char AuditIsDeleted { get; set; }

        public int AlgorithmId { get; set; }
        public int SupportedLangId { get; set; }

        public Algorithm Algorithm { get; set; } = null!;
        public SupportedLang SupportedLang { get; set; } = null!;
    }
}
