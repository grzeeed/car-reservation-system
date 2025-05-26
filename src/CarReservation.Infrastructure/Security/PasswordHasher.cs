namespace CarReservation.Infrastructure.Security;

using CarReservation.Application.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8; // 128 bit
    private const int KeySize = 256 / 8; // 256 bit
    private const int Iterations = 10000;
    private const char Delimiter = ';';

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        var salt = GenerateSalt();
        var hash = GenerateHash(password, salt);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
            return false;

        try
        {
            var parts = passwordHash.Split(Delimiter);
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var testHash = GenerateHash(password, salt);

            return SlowEquals(hash, testHash);
        }
        catch
        {
            return false;
        }
    }

    private static byte[] GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);
        return salt;
    }

    private static byte[] GenerateHash(string password, byte[] salt)
    {
        return KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: KeySize
        );
    }

    private static bool SlowEquals(byte[] a, byte[] b)
    {
        var diff = (uint)a.Length ^ (uint)b.Length;
        for (var i = 0; i < a.Length && i < b.Length; i++)
            diff |= (uint)(a[i] ^ b[i]);
        return diff == 0;
    }
}
