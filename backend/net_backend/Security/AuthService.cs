using Microsoft.AspNetCore.Identity;

public class AuthService
{
    private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword("", password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        return _passwordHasher.VerifyHashedPassword("", hash, password) == PasswordVerificationResult.Success;
    }
}