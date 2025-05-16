using System;
using System.ComponentModel.DataAnnotations;

namespace coderepo_api.Models
{
    public class Algorithm
    {
        [Key]
        public int algorithm_id { get; set; }
        public int owner_id { get; set; }
        public string title { get; set; } = null!;
        public string description { get; set; } = null!;
        public AlgorithmMeta Meta { get; set; } = null!;
        public ICollection<AlgorithmLang> Langs { get; set; } = new List<AlgorithmLang>();
            
    }

}
