namespace coderepo_api.Dtos
{
    public class AssignTagsDto
    {
        public int AlgorithmId { get; set; }
        public List<int> TagIds { get; set; } = [];
    }
}
