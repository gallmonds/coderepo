using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Primitives;

namespace coderepo_api.Models
{
    public class AlgorithmLang
    {
        [Key]
        public int algorithm_id { get; set; }
        public Algorithm Algorithm { get; set; } = null!;
        public int lang_id { get; set; }
        public string rootlang_path { get; set; }
        public char audit_isdeleted { get; set; }
        
    }
}