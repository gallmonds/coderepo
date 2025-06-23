namespace coderepo_api.Models
{
    public class AlgorithmChangelog
    {
        public int changelog_id { get; set; }
        public int algorithm_id { get; set; }
        public string file_path { get; set; } = null!;
        public int lang_id { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;

    }
}
