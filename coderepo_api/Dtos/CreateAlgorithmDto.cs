namespace coderepo_api.Dtos
{
    public class CreateAlgorithmDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPrivate { get; set; }
    }
}
