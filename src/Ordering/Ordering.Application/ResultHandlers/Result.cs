namespace Ordering.Application.ResultHandlers
{
    public class Result<T, TError> where TError : class
    {
        public bool Success => Error is null;

        public T Data { get; private set; }

        public TError Error { get; private set; }

        public Result(T data) => Data = data;

        public Result(TError error) => Error = error;

        public static implicit operator T(Result<T, TError> result) => result.Data;

        public static implicit operator TError(Result<T, TError> result) => result.Error;

        public static implicit operator Result<T, TError>(T data) => new Result<T, TError>(data);

        public static implicit operator Result<T, TError>(TError error) => new Result<T, TError>(error);
    }
}
