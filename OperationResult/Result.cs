using System;
using OperationResult.Tags;

namespace OperationResult
{
    /// <summary>
    /// Result of operation (without Error field)
    /// </summary>
    /// <typeparam name="TResult">Type of Value field</typeparam>
    public struct Result<TResult> : IEquatable<Result<TResult>>
    {
        private readonly bool _isSuccess;

        public readonly TResult Value;

        public bool IsSuccess => _isSuccess;
        public bool IsError => !_isSuccess;

        private Result(bool isSuccess)
        {
            _isSuccess = isSuccess;
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

        public static bool operator ==(Result<TResult> r1, Result<TResult> r2) 
        {
            return Helpers.Equals((r1._isSuccess, r1.Value, 1), (r2._isSuccess, r2.Value, 1));
        }

        public static bool operator !=(Result<TResult> r1, Result<TResult> r2) 
        {
            return !(r1 == r2);
        }

        public override int GetHashCode()
        {
            return this.IsSuccess ? this.Value.GetHashCode() : this.IsSuccess.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Result<TResult> t) 
            {
                return this == t;
            }
            return false;
        }

        public bool Equals(Result<TResult> other)
        {
            return this == other;
        }
    }

    /// <summary>
    /// Result of operation (with Error field)
    /// </summary>
    /// <typeparam name="TResult">Type of Value field</typeparam>
    /// <typeparam name="TError">Type of Error field</typeparam>
    public struct Result<TResult, TError> : IEquatable<Result<TResult, TError>>
    {
        private readonly bool _isSuccess;

        public readonly TResult Value;
        public readonly TError Error;

        public bool IsSuccess => _isSuccess;
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

        public void Deconstruct(out TResult result, out TError error)
        {
            result = Value;
            error = Error;
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

        public static bool operator ==(Result<TResult, TError> r1, Result<TResult, TError> r2) 
        {
            return Helpers.Equals((r1._isSuccess, r1.Value, r1.Error), (r2._isSuccess, r2.Value, r2.Error));
        }

        public static bool operator !=(Result<TResult, TError> r1, Result<TResult, TError> r2) 
        {
            return !(r1 == r2);
        }

        public override int GetHashCode()
        {
            return this.IsSuccess ? this.Value.GetHashCode() : this.IsSuccess.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Result<TResult, TError> t) 
            {
                return this == t;
            }
            return false;
        }

        public bool Equals(Result<TResult, TError> other)
        {
            return this == other;
        }
    }

    /// <summary>
    /// Result of operation (with different Errors)
    /// </summary>
    /// <typeparam name="TResult">Type of Value field</typeparam>
    /// <typeparam name="TError1">Type of first Error</typeparam>
    /// <typeparam name="TError2">Type of second Error</typeparam>
    public struct Result<TResult, TError1, TError2>: IEquatable<Result<TResult, TError1, TError2>>
    {
        private readonly bool _isSuccess;

        public readonly TResult Value;
        public readonly object Error;

        public bool IsSuccess => _isSuccess;
        public bool IsError => !_isSuccess;

        public bool HasError<TError>() => Error is TError;
        public TError GetError<TError>() => (TError)Error;

        private Result(TResult result)
        {
            _isSuccess = true;
            Value = result;
            Error = null;
        }

        private Result(object error)
        {
            _isSuccess = false;
            Value = default(TResult);
            Error = error;
        }

        public void Deconstruct(out TResult result, out object error)
        {
            result = Value;
            error = Error;
        }

        public static implicit operator bool(Result<TResult, TError1, TError2> result)
        {
            return result._isSuccess;
        }

        public static implicit operator Result<TResult, TError1, TError2>(TResult result)
        {
            return new Result<TResult, TError1, TError2>(result);
        }

        public static implicit operator Result<TResult, TError1, TError2>(SuccessTag<TResult> tag)
        {
            return new Result<TResult, TError1, TError2>(tag.Value);
        }

        public static implicit operator Result<TResult, TError1, TError2>(ErrorTag<TError1> tag)
        {
            return new Result<TResult, TError1, TError2>(tag.Error);
        }

        public static implicit operator Result<TResult, TError1, TError2>(ErrorTag<TError2> tag)
        {
            return new Result<TResult, TError1, TError2>(tag.Error);
        }

        public static bool operator ==(Result<TResult, TError1, TError2> r1, Result<TResult, TError1, TError2> r2) 
        {
            return Helpers.Equals((r1._isSuccess, r1.Value, r2.Error), (r2._isSuccess, r2.Value, r2.Error));
        }

        public static bool operator !=(Result<TResult, TError1, TError2> r1, Result<TResult, TError1, TError2> r2) 
        {
            return !(r1 == r2);
        }

        public override int GetHashCode()
        {
            return this.IsSuccess ? this.Value.GetHashCode() : this.IsSuccess.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Result<TResult, TError1, TError2> t) 
            {
                return this == t;
            }
            return false;
        }

        public bool Equals(Result<TResult, TError1, TError2> other)
        {
            return this == other;
        }
    }

    /// <summary>
    /// Result of operation (with different Errors)
    /// </summary>
    /// <typeparam name="TResult">Type of Value field</typeparam>
    /// <typeparam name="TError1">Type of first Error</typeparam>
    /// <typeparam name="TError2">Type of second Error</typeparam>
    /// <typeparam name="TError3">Type of third Error</typeparam>
    public struct Result<TResult, TError1, TError2, TError3> : IEquatable<Result<TResult, TError1, TError2, TError3>>
    {
        private readonly bool _isSuccess;

        public readonly TResult Value;
        public readonly object Error;

        public bool IsSuccess => _isSuccess;
        public bool IsError => !_isSuccess;

        public bool HasError<TError>() => Error is TError;
        public TError GetError<TError>() => (TError)Error;

        private Result(TResult result)
        {
            _isSuccess = true;
            Value = result;
            Error = null;
        }

        private Result(object error)
        {
            _isSuccess = false;
            Value = default(TResult);
            Error = error;
        }

        public void Deconstruct(out TResult result, out object error)
        {
            result = Value;
            error = Error;
        }

        public static implicit operator bool(Result<TResult, TError1, TError2, TError3> result)
        {
            return result._isSuccess;
        }

        public static implicit operator Result<TResult, TError1, TError2, TError3>(TResult result)
        {
            return new Result<TResult, TError1, TError2, TError3>(result);
        }

        public static implicit operator Result<TResult, TError1, TError2, TError3>(SuccessTag<TResult> tag)
        {
            return new Result<TResult, TError1, TError2, TError3>(tag.Value);
        }

        public static implicit operator Result<TResult, TError1, TError2, TError3>(ErrorTag<TError1> tag)
        {
            return new Result<TResult, TError1, TError2, TError3>(tag.Error);
        }

        public static implicit operator Result<TResult, TError1, TError2, TError3>(ErrorTag<TError2> tag)
        {
            return new Result<TResult, TError1, TError2, TError3>(tag.Error);
        }

        public static implicit operator Result<TResult, TError1, TError2, TError3>(ErrorTag<TError3> tag)
        {
            return new Result<TResult, TError1, TError2, TError3>(tag.Error);
        }

        public static bool operator ==(Result<TResult, TError1, TError2, TError3> r1, Result<TResult, TError1, TError2, TError3> r2) 
        {
            return Helpers.Equals((r1._isSuccess, r1.Value, r2.Error), (r2._isSuccess, r2.Value, r2.Error));
        }

        public static bool operator !=(Result<TResult, TError1, TError2, TError3> r1, Result<TResult, TError1, TError2, TError3> r2) 
        {
            return !(r1 == r2);
        }

        public override int GetHashCode()
        {
            return this.IsSuccess ? this.Value.GetHashCode() : this.IsSuccess.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Result<TResult, TError1, TError2> t) 
            {
                return this == t;
            }
            return false;
        }

        public bool Equals(Result<TResult, TError1, TError2, TError3> other)
        {
            return this == other;
        }
    }
}
