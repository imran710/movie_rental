namespace Core.Infrastructure.Option;

public record ApiOption
{
    public const string SectionName = "Api";

    public required OpenApiOption OpenApi { get; init; }
    public required SpoonacularOption Spoonacular { get; init; }
    public required OpenAiOption OpenAi { get; init; }
    public required EdamamOption Edamam { get; init; }
    public required string BarcodeScanner { get; init; }

    public record OpenApiOption
    {
        public static readonly string SectionName = $"{ApiOption.SectionName}:OpenApi";

        public required string Title { get; init; }
        public required string OpenApiRoutePattern { get; init; }
        public required string ScalarEndpointRoutePattern { get; init; }
        public required string ServerUrl { get; init; }
        public string SubPath => string.IsNullOrWhiteSpace(ServerUrl) ? string.Empty : new Uri(ServerUrl).AbsolutePath;
    }

    public record SpoonacularOption
    {
        public static readonly string SectionName = $"{ApiOption.SectionName}:Spoonacular";

        public required string BaseUrl { get; init; }
        public required string ApiKey { get; init; }
    }

    public record OpenAiOption
    {
        public static readonly string SectionName = $"{ApiOption.SectionName}:OpenAi";

        public required string ApiKey { get; init; }
    }

    public record EdamamOption
    {
        public static readonly string SectionName = $"{ApiOption.SectionName}:Edamam";

        public required string BaseUrl { get; init; }
        public required string ApiKey { get; init; }
        public required string AppId { get; init; }
    }
}
