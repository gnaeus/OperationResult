using OperationResult.Tags;

namespace OperationResult
{
    /// <summary>
    /// Result of operation (without Error field)
    /// </summary>
    /// <typeparam name="TResult">Type of Value field</typeparam>
    public struct Result<TResult>
    {
        private readonly bool _isSuccess;

        public readonly TResult Value;

        public bool IsSuccsess => _isSuccess;
        public bool IsError => !_isSuccess;

        private Result(bool isSuccsess)
        {
            _isSuccess = isSuccsess;
            Value = default(TResult);
        }

        private Result(TResult result)
        {
            _isSuccess = true;
            Value = result;
        }

        public static implicit operator bool(Result<TResult> result)
        {
            return result._isSuccess;
        }

        public static implicit operator Result<TResult>(TResult result)
        {
            return new Result<TResult>(result);
        }

        public static implicit operator Result<TResult>(SuccessTag<TResult> tag)
        {
            return new Result<TResult>(tag.Value);
        }

        private static Result<TResult> ErrorResult = new Result<TResult>(false);

        public static implicit operator Result<TResult>(ErrorTag tag)
        {
            return ErrorResult;
        }
    }

    /// <summary>
    /// Result of operation (with Error field)
    /// </summary>
    /// <typeparam name="TResult">Type of Value field</typeparam>
    /// <typeparam name="TError">Type of Error field</typeparam>
    public struct Result<TResult, TError>
    {
        private readonly bool _isSuccess;

        public readonly TResult Value;
        public readonly TError Error;

        public bool IsSuccsess => _isSuccess;
        public bool IsError => !_isSuccess;

        private Result(TResult result)
        {
            _isSuccess = true;
            Value = result;
            Error = default(TError);
        }

        private Result(TError error)
        {
            _isSuccess = false;
            Value = default(TResult);
            Error = error;
        }

        public static implicit operator bool(Result<TResult, TError> result)
        {
            return result._isSuccess;
        }

        public static implicit operator Result<TResult, TError>(TResult result)
        {
            return new Result<TResult, TError>(result);
        }

        public static implicit operator Result<TResult, TError>(SuccessTag<TResult> tag)
        {
            return new Result<TResult, TError>(tag.Value);
        }

        public static implicit operator Result<TResult, TError>(ErrorTag<TError> tag)
        {
            return new Result<TResult, TError>(tag.Error);
        }
    }
}
