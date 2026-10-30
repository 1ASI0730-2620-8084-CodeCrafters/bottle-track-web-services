namespace BottleTrack.API.Shared.Application.Patterns;

public class Result<TValue, TError>
{
    private readonly TValue? value;
    private readonly TError? error;

    public bool IsSuccess { get; }

    public bool IsFailure
    {
        get { return !IsSuccess; }
    }

    public TValue Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("A failed result has no value.");

            return value!;
        }
    }

    public TError Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("A successful result has no error.");

            return error!;
        }
    }

    private Result(bool isSuccess, TValue? value, TError? error)
    {
        IsSuccess = isSuccess;
        this.value = value;
        this.error = error;
    }

    public static Result<TValue, TError> Success(TValue value)
    {
        return new Result<TValue, TError>(true, value, default);
    }

    public static Result<TValue, TError> Failure(TError error)
    {
        return new Result<TValue, TError>(false, default, error);
    }

    public TResult Fold<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onFailure)
    {
        if (IsSuccess)
            return onSuccess(value!);

        return onFailure(error!);
    }
}
