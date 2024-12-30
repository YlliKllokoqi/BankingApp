namespace BankingApp.Application.DTOs;

public class ResultDto<T>
{
    public bool IsSuccess { get; private set; }
    public T Data { get; private set; }
    public ErrorResponseDto Error { get; set; }

    public ResultDto(bool isSuccess, T data, ErrorResponseDto error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static ResultDto<T> Success(T data) => new(true, data, null);
    public static ResultDto<T> Failure(ErrorResponseDto error) => new(false, default, error);
}