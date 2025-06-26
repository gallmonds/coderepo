namespace coderepo_api.Dtos
{
    public class AddCollaboratorsDto
    {
        public List<int> UserIds { get; set; } = new List<int>();
        public int AlgorithmId { get; set; }
    }
}
