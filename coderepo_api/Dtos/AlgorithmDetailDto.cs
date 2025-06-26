namespace coderepo_api.Dtos
{
    public class AlgorithmDetailDto : AlgorithmSummaryDto
    {
        public new List<LanguageDetailDto> Languages { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = [];
    }
}
