using Library.Results;

namespace Core.Infrastructure.Email.Common;

public static class EmailErrors
{
    public static readonly Error SendEmailFailed = Error.Unexpected("Failed to send email", code: "Error.Email.SendEmailFailed");
}
