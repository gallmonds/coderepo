namespace coderepo_api.Models
{
    public class AlgorithmMeta
    {
        public int algorithm_id { get; set; }
        public string root_path { get; set; } = null!;
        public char isprivate { get; set; }
        public char isflagged { get; set; }
        public char isdisabled { get; set; }
        public DateTime created_at { get; set; }
    }
}
