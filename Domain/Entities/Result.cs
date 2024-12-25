namespace BankingApp.Domain.Entities;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T Data { get; private set; }
    public ErrorResponse Error { get; set; }

    public Result(bool isSuccess, T data, ErrorResponse error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(ErrorResponse error) => new(false, default, error);
}