namespace Library.Results;

public class ResultInvalidOperationException(string message) : ResultException(message, "Error.Result.InvalidOperation");
