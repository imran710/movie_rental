namespace Core.Infrastructure.Option;

public record PaymentOption
{
    public static readonly string SectionName = "Payment";

    public required StripeOption Stripe { get; init; }

    public record StripeOption
    {
        public static readonly string SectionName = $"{PaymentOption.SectionName}:Stripe";

        public required string PublishableKey { get; init; }
        public required string SecretKey { get; init; }
        public required string EndpointSecret { get; init; }
    }
}
