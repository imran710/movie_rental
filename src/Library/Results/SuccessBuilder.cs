namespace Library.Results;

public class SuccessBuilder
{
  public static Success Default => default;
  public static Success<T> Success<T>(string message = "Success", T? data = default) => new(message, data);
}
