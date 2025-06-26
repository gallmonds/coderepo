namespace coderepo_api.Dtos
{
    public class AlgorithmSummaryDto
    {
        public int AlgorithmId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public UserSummaryDto Owner { get; set; } = new UserSummaryDto();
        public int RatingCount { get; set; }
        public int CommentCount { get; set; }
        public List<string> Tags { get; set; } = [];
        public List<LanguageSummaryDto> Languages { get; set; } = [];
    }
}
