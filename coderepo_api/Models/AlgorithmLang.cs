namespace coderepo_api.Models
{
    public class AlgorithmLang
    {
        public int algorithm_id { get; set; }
        public int lang_id { get; set; }
        public string rootlang_path { get; set; } = null!;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
