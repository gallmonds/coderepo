namespace coderepo_api.Dtos;

public class CreateAlgorithmDto
{
    public string algorithm_title { get; set; } = string.Empty;
    public string algorithm_description { get; set; } = string.Empty;
    public char algorithm_isprivate { get; set; } 
    public int dbuser_id { get; set; } 

}