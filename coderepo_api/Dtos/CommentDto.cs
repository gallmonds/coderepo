namespace coderepo_api.Dtos
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PfpRoute { get; set; } = string.Empty;
        public int RatingCount { get; set; }
        public List<CommentDto> Replies { get; set; } = [];

    }
}
