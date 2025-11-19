using Core.Infrastructure.Email.Common;

using FluentEmail.Core;

namespace Core.Infrastructure.Email.Smtp;

public class EmailSmtp(IFluentEmail fluentEmail) : IEmail
{
    public async Task<Result<Success>> SendEmailAsync(IEnumerable<string> to, string subject, EmailBody emailBody, IEnumerable<string>? bcc = null, IEnumerable<string>? cc = null)
    {
        var email = to.Aggregate(fluentEmail, (current, item) => current.To(item));

        if (bcc is not null)
            email = bcc.Aggregate(email, (current, item) => current.BCC(item));

        if (cc is not null)
            email = cc.Aggregate(email, (current, item) => current.CC(item));

        var result = await email
            .Subject(subject)
            .Body(emailBody.Body, isHtml: emailBody.Type == EmailBody.BodyType.Html)
            .SendAsync()
            .ConfigureAwait(false);
        if (!result.Successful)
        {
            return EmailErrors.SendEmailFailed;
        }

        return SuccessBuilder.Default;
    }
}
