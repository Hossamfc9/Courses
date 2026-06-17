using MediatR;

namespace Application.Common;

public class RequestResult<TResult>
{
    public TResult? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public ErrorCode? Code { get; set; }

    public static RequestResult<TResult> Success(TResult data, string message = "")
    {
        return new RequestResult<TResult>()
        {
            Data = data,
            IsSuccess = true,
            Message = message,
            Code = null
        };
    }
    public static  RequestResult<TResult> Failure(ErrorCode errorCode, string message = "")
    {
        return new RequestResult<TResult>()
        {
            Code = errorCode,
            IsSuccess = false,
            Message = message,
        };
    }
}