namespace coderepo_api.Dtos;

public class CreateUserDto
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string password_hash { get; set; } = string.Empty;
    public string password_salt { get; set; } = string.Empty;
}