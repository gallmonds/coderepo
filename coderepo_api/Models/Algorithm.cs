namespace coderepo_api.Models
{
    public class Algorithm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public char AuditIsDeleted { get; set; }

        public int OwnerId { get; set; }

        public DbUser UserDb { get; set; } = null!;

        public AlgorithmMeta? AlgorithmMeta { get; set; }

        public List<AlgorithmLang> AlgorithmLangs { get; set; } = new List<AlgorithmLang>();

        public List<TagAlgorithm> AlgorithmTags { get; set; } = new List<TagAlgorithm>();
        public List<AlgorithmCollaborator> AlgorithmCollaborators { get; set; } = new List<AlgorithmCollaborator>();
        public List<AlgorithmChangelog> AlgorithmChangelogs { get; set; } = new List<AlgorithmChangelog>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
