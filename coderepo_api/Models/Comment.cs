namespace coderepo_api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public char AuditIsDeleted { get; set; } 
        public int ContentId { get; set; }



        public int OwnerId { get; set; }
        public int TypeId { get; set; }
        public int? ReplyToId { get; set; }

        

        public DbUser Owner { get; set; } = null!;
        public ContentType Type { get; set; } = null!;
        public Comment? ReplyTo { get; set; } = null!;
        public List<Comment> Replies { get; set; } = new();
    }
}