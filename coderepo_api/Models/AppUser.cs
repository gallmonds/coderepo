using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace coderepo_api.Models
{
    public class AppUser
    {
        [Key]
        public int user_id { get; set; }
        public int pfp_id { get; set; }
        public string username { get; set; } = null!;
        public string email { get; set; } = null!;
        public string password_hash { get; set; } = null!;
        public string password_salt { get; set; } = null!;
        public char isflagged { get; set; }
        public char isbanned { get; set; }
        public char audit_isdeleted { get; set; }
    }
}
