using System.Security.Cryptography;
using BottleTrack.API.Iam.Application.Services;

namespace BottleTrack.API.Iam.Infrastructure.Hashing;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100000;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(key);
    }

    public bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split('.');
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expectedKey = Convert.FromBase64String(parts[1]);
        byte[] actualKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

        return CryptographicOperations.FixedTimeEquals(expectedKey, actualKey);
    }
}
