using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Domain.Option;
using Core.Features.Users.Entity;

namespace Core.Features.Auth.OtpVerifications;

public record OtpVerification : IEntity, ICreationEntity, IDeletableEntity
{
    public const int MinLength = 3;
    public const int MaxLength = 50;

    public long Id { get; }

    public required long UserId { get; init; }
    public User? User { get; private set; }

    public required string Otp { get; init; }
    public required string Purpose { get; init; }
    public bool IsVerified { get; private set; }
    public DateTimeOffset ExpiresAt { get; init; }
    public int AttemptCount { get; private set; }

    public CreationTime CreationTime { get; } = CreationTime.Create();
    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();

    public bool IsExpired() => DateTimeHelperStatic.UtcNow > ExpiresAt;

    public Result<Success> VerifyOtp(string inputOtp, DomainOption.OtpVerificationOption otpVerificationOption)
    {
        if (IsExpired())
            return OtpVerificationErrors.OtpExpired;

        if (IsVerified)
            return OtpVerificationErrors.AlreadyVerified;

        if (Otp != inputOtp)
            return IncrementAttemptCount();

        IsVerified = true;

        return SuccessBuilder.Default;
    }

    private Result<Success> IncrementAttemptCount()
    {
        AttemptCount++;
        return OtpVerificationErrors.InvalidOtp;
    }

    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }

    // Factory
    public static OtpVerification CreateForPasswordReset(User user, DomainOption.OtpVerificationOption verificationOption, string? otp = null)
    {
        return CreateNew(user, "PasswordReset", verificationOption, otp);
    }

    public static OtpVerification CreateForLogin(User user, DomainOption.OtpVerificationOption verificationOption, string? otp = null)
    {
        return CreateNew(user, "Login", verificationOption, otp);
    }

    private static OtpVerification CreateNew(User user, string purpose, DomainOption.OtpVerificationOption verificationOption, string? otp = null)
    {
        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("Purpose cannot be null or empty.", nameof(purpose));

        if (verificationOption.OtpValidityDurationInMinutes <= 0)
            throw new ArgumentException("OTP validity duration must be greater than zero.");

        return new OtpVerification
        {
            UserId = user.Id,
            User = user,
            Otp = otp ?? OtpVerificationHelper.GenerateOtp(verificationOption.OtpLength, verificationOption.IsOtpAlphanumeric),
            Purpose = purpose,
            ExpiresAt = DateTimeHelperStatic.UtcNow.AddMinutes(verificationOption.OtpValidityDurationInMinutes),
            IsVerified = false,
            AttemptCount = 0,
        };
    }
}
