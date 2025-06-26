namespace coderepo_api.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public char IsDeleted { get; set; }

        public List<DbUser> Users { get; set; } = new List<DbUser>();
        public List<SupportedLang> Supportedlangs { get; set; } = new List<SupportedLang>();
    }
}
