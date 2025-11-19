namespace Core.Infrastructure.Email.Common;

public readonly record struct EmailBody
{
    public BodyType Type { get; init; }
    public string Body { get; init; }

    private EmailBody(string body, BodyType bodyType)
    {
        Type = bodyType;
        Body = body;
    }

    public static EmailBody CreateHtml(string body) => new(body, BodyType.Html);
    public static EmailBody CreateText(string body) => new(body, BodyType.Text);

    public enum BodyType
    {
        Text,
        Html,
    }
}
