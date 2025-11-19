using Core.Infrastructure.Email.Common;

namespace Core.Infrastructure.Email;

public interface IEmail
{
    /// <summary>
    /// Send email using HTML
    /// </summary>
    /// <param name="to">To email id list</param>
    /// <param name="subject">Subject of email</param>
    /// <param name="emailBody">Html/Text content of the email body</param>
    /// <param name="bcc">BCC email id list</param>
    /// <param name="cc">CC email id list</param>
    /// <returns></returns>
    Task<Result<Success>> SendEmailAsync(
        IEnumerable<string> to,
        string subject,
        EmailBody emailBody,
        IEnumerable<string>? bcc = default,
        IEnumerable<string>? cc = default);
}
