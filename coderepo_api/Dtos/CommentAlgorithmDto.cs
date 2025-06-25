namespace coderepo_api.Dtos
{
    public class CommentAlgorithmDto
    {
        public int ContentId { get; set; }
        public string Body { get; set; }
        public int? ReplyToId { get; set; } = null;
    }
}
