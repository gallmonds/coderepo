namespace coderepo_api.Dtos
{
    public class AddLanguageFormDto
    {
        public int AlgorithmId { get; set; }
        public int SupportedLangId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
