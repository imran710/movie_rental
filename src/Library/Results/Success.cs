namespace Library.Results;

public readonly record struct Success;
public readonly record struct Success<T>(string Message, T? Data);
