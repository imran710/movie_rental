using Core.Infrastructure.Email.Smtp;

namespace Core.Infrastructure.Email.Common;

public record EmailOption
{
    public const string SectionName = "Email";
     
    public required EmailSmtpOption Smtp { get; init; }
}
