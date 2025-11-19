namespace Library.Results;

public class ResultException(string message, string code = "Error.Result.Exception") : Exception(message)
{
    public string Code { get; init; } = code;
}
