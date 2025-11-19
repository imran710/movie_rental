using System.Security.Cryptography;
using System.Text;

using Core.Domain.Helper.Enums;
using Core.Domain.Option;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Core.Domain.Helper;

public class SecurityHelper(IOptions<DomainOption.SecurityOption> options)
{
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const int NumberOfIterations = 10000;
    private static readonly RandomNumberGenerator DefaultRng = RandomNumberGenerator.Create();
    private const int IterationCount = 100_000;

    public string Encrypt(string plainText)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(options.Value.AesSecretKey, salt, NumberOfIterations, HashAlgorithmName.SHA256);
        byte[] key = rfc2898DeriveBytes.GetBytes(KeySize / 8);
        byte[] iv = rfc2898DeriveBytes.GetBytes(BlockSize / 8);

        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor(key, iv);
        using var memoryStream = new MemoryStream();
        memoryStream.Write(salt, 0, salt.Length);
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
        }
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string encryptedText)
    {
        byte[] cipherBytes = Convert.FromBase64String(encryptedText);
        byte[] salt = new byte[16];
        Array.Copy(cipherBytes, 0, salt, 0, 16);

        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(options.Value.AesSecretKey, salt, NumberOfIterations, HashAlgorithmName.SHA256);
        byte[] key = rfc2898DeriveBytes.GetBytes(KeySize / 8);
        byte[] iv = rfc2898DeriveBytes.GetBytes(BlockSize / 8);

        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor(key, iv);
        using var memoryStream = new MemoryStream(cipherBytes, 16, cipherBytes.Length - 16);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }

    #region Public Functions
    public string HashPassword(string password)
    {
        return Convert.ToBase64String(HashPasswordV3(password, DefaultRng));
    }

    public UserPasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var result = VerifyThatHashedPassword(hashedPassword, providedPassword);
        return result switch
        {
            PasswordVerificationResult.Failed => UserPasswordVerificationResult.Failed,
            PasswordVerificationResult.Success => UserPasswordVerificationResult.Success,
            PasswordVerificationResult.SuccessRehashNeeded => UserPasswordVerificationResult.Success,
            _ => UserPasswordVerificationResult.Failed,
        };
    }
    #endregion

    #region Private functions
    private static PasswordVerificationResult VerifyThatHashedPassword(string hashedPassword, string providedPassword)
    {
        byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

        if (decodedHashedPassword.Length == 0)
        {
            return PasswordVerificationResult.Failed;
        }
        switch (decodedHashedPassword[0])
        {
            case 0x01:
                if (VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out int embeddedIterCount, out KeyDerivationPrf prf))
                {
                    if (embeddedIterCount < IterationCount)
                    {
                        return PasswordVerificationResult.SuccessRehashNeeded;
                    }

                    return prf is KeyDerivationPrf.HMACSHA1 or KeyDerivationPrf.HMACSHA256
                        ? PasswordVerificationResult.SuccessRehashNeeded
                        : PasswordVerificationResult.Success;
                }
                else
                {
                    return PasswordVerificationResult.Failed;
                }

            default:
                return PasswordVerificationResult.Failed;
        }
    }

    private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount, out KeyDerivationPrf prf)
    {
        iterCount = default;
        prf = default;

        try
        {
            prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
            iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
            int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

            if (saltLength < 128 / 8)
            {
                return false;
            }
            byte[] salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

            int subKeyLength = hashedPassword.Length - 13 - salt.Length;
            if (subKeyLength < 128 / 8)
            {
                return false;
            }
            byte[] expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subKeyLength);
            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        }
        catch
        {
            return false;
        }
    }

    private static byte[] HashPasswordV3(
        string password,
        RandomNumberGenerator rng,
        KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA512,
        int iterCount = IterationCount,
        int saltSize = 128 / 8,
        int numBytesRequested = 256 / 8)
    {
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        outputBytes[0] = 0x01;
        WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
        WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
        WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
        return outputBytes;
    }

    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
            | ((uint)(buffer[offset + 1]) << 16)
            | ((uint)(buffer[offset + 2]) << 8)
            | ((uint)(buffer[offset + 3]));
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }
    #endregion
}

