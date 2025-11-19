using System.Net;
using System.Net.Mail;

using Core.Infrastructure.Email.Common;
using Core.Infrastructure.Email.Smtp;
using Core.Infrastructure.ServiceInjector;

using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Email;

public class ConfigureEmailService : IInjectServicesWithConfiguration
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailOption>().BindConfiguration(EmailOption.SectionName); 
        services.AddOptions<EmailSmtpOption>().BindConfiguration(EmailSmtpOption.SectionName);

        var smtpOption = configuration.GetSection(EmailSmtpOption.SectionName).Get<EmailSmtpOption>() ?? throw new InvalidOperationException($"SMTP option is not configured in {EmailSmtpOption.SectionName}");

        services.AddScoped<ISender>(_ => new SmtpSender(new SmtpClient(smtpOption.Host)
        {
            Port = int.Parse(smtpOption.Port),
            Credentials = new NetworkCredential(smtpOption.Username, smtpOption.Password),
            EnableSsl = true,
            Timeout = 100000,
        }));
        services
            .AddFluentEmail(smtpOption.From, smtpOption.Name)
            .AddSmtpSender(smtpOption.Host, int.Parse(smtpOption.Port), smtpOption.Username, smtpOption.Password);
        services.AddScoped<IEmail, EmailSmtp>();
    }
}
