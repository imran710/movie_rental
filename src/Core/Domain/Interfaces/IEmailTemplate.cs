using Core.Infrastructure.Email.Common;

namespace Core.Domain.Interfaces;

public interface IEmailTemplate
{
    string Subject { get; }
    EmailBody Body { get; }
}
