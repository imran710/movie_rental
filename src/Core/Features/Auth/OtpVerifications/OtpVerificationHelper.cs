namespace Core.Features.Auth.OtpVerifications;

public static class OtpVerificationHelper
{
    private const string NumericCharacters = "0123456789";
    private const string AlphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    /// <summary>
    /// Generates a random OTP of the specified length.
    /// </summary>
    /// <param name="length">The length of the OTP.</param>
    /// <param name="isAlphanumeric">Whether the OTP should include alphanumeric characters.</param>
    /// <returns>A randomly generated OTP.</returns>
    /// <exception cref="ArgumentException">Thrown when the length is less than or equal to zero.</exception>
    public static string GenerateOtp(int length, bool isAlphanumeric = false)
    {
        if (length <= 0)
            throw new ArgumentException("Length must be greater than zero.", nameof(length));

        var characters = isAlphanumeric ? AlphanumericCharacters : NumericCharacters;
        var random = new Random();
        return new string(Enumerable.Range(0, length)
                                    .Select(_ => characters[random.Next(characters.Length)])
                                    .ToArray());
    }
}
