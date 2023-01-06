using weitus_backend.Data.Dto;

namespace weitus_backend;

public class Result
{
    public bool Success { get; set; }
    public string[] Errors { get; set; }

    protected Result(bool success, string[] errors)
    {
        Success = success;
        Errors = errors;
    }

    public static Result Ok()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Err(params string[] errors)
    {
        return new Result(false, errors);
    }

    public static implicit operator ErrorResponse(Result result)
    {
        if (result.Success)
        {
            throw new InvalidOperationException("Cannot convert a successful result to an error response");
        }

        return new ErrorResponse() { Message = result.Errors.Aggregate((a, b) => $"{a}, {b}") };
    }
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected Result(bool success, string[] errors, T? value) : base(success, errors)
    {
        Value = value;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, Array.Empty<string>(), value);
    }

    public static new Result<T> Err(params string[] errors)
    {
        return new Result<T>(false, errors, default);
    }

    public static implicit operator T(Result<T> result)
    {
        if (!result.Success)
        {
            throw new InvalidOperationException("Cannot convert an unsuccessful result to a value");
        }

        return result.Value;
    }
}
