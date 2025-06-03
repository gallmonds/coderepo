namespace coderepo_api.Models
{
    public class Algorithm
    {
        public int algorithm_id { get; set; }
        public int owner_id { get; set; }
        public string title { get; set; } = null!;
        public string description { get; set; } = null!;
    }
}
