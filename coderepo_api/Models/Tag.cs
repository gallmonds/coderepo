namespace coderepo_api.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public char AuditIsDeleted { get; set; }

        public List<TagAlgorithm> AlgorithmTags { get; set; } = new List<TagAlgorithm>();
    }
}
