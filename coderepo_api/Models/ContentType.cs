namespace coderepo_api.Models
{
    public class ContentType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public char AuditIsDeleted { get; set; }
   
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
