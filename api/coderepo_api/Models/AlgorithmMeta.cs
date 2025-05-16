using System;
using System.ComponentModel.DataAnnotations;

namespace coderepo_api.Models
{
    public class AlgorithmMeta
    {
        [Key]
        public int algorithm_id { get; set; }
        public Algorithm Algorithm { get; set; } = null!;
        public string root_path { get; set; } = null!;
        public char isprivate { get; set; }
        public char isflagged { get; set; }
        public char isdisabled { get; set; }
        public char audit_isdeleted { get; set; }
    }
}