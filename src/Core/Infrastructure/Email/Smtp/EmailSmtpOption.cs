using Core.Infrastructure.Email.Common;

namespace Core.Infrastructure.Email.Smtp;

public record EmailSmtpOption
{
    public const string SectionName = $"{EmailOption.SectionName}:{nameof(EmailOption.Smtp)}";

    public required string From { get; init; }
    public required string Name { get; init; }
    public required string Host { get; init; }
    public required string Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}
