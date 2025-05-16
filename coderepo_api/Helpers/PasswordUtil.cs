using System.Security.Cryptography;
using System.Text;

public static class PasswordUtil
{
    public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using var hmac = new HMACSHA512();
	    var saltBytes = hmac.Key;
	    var hashBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        passwordSalt = Convert.ToBase64String(saltBytes);
        passwordHash = Convert.ToBase64String(hashBytes);
    }
}
