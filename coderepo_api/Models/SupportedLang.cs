namespace coderepo_api.Models
{
    public class SupportedLang
    {
        public int Id { get; set; }
        public string LangName { get; set; } = null!;
        public string LangExtension { get; set; } = null!;
        public char AuditIsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public int IconId { get; set; }

        public Media MediaDb { get; set; } = null!;

        public List<AlgorithmLang> AlgorithmLangs { get; set; } = new List<AlgorithmLang>();
        public List<AlgorithmChangelog> AlgorithmChangelogs { get; set; } = new List<AlgorithmChangelog>();
    }
}
