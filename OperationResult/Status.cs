using OperationResult.Tags;

namespace OperationResult
{
    /// <summary>
    /// Status of operation (without Value and Error fields)
    /// </summary>
    public struct Status
    {
        private readonly bool _isSuccess;

        public bool IsSuccsess => _isSuccess;
        public bool IsError => !_isSuccess;

        private Status(bool isSuccsess)
        {
            _isSuccess = isSuccsess;
        }

        public static implicit operator bool(Status status)
        {
            return status._isSuccess;
        }

        private static Status SuccessStatus = new Status(true);

        public static implicit operator Status(SuccessTag tag)
        {
            return SuccessStatus;
        }

        private static Status ErrorStatus = new Status(false);

        public static implicit operator Status(ErrorTag tag)
        {
            return ErrorStatus;
        }
    }

    /// <summary>
    /// Status of operation (without Value but with Error field)
    /// </summary>
    /// <typeparam name="TError">Type of Error field</typeparam>
    public struct Status<TError>
    {
        private readonly bool _isSuccess;

        public readonly TError Error;

        public bool IsSuccsess => _isSuccess;
        public bool IsError => !_isSuccess;

        private Status(bool isSuccsess)
        {
            _isSuccess = isSuccsess;
            Error = default(TError);
        }

        private Status(TError error)
        {
            _isSuccess = false;
            Error = error;
        }

        public static implicit operator bool(Status<TError> status)
        {
            return status._isSuccess;
        }

        private static Status<TError> SuccessStatus = new Status<TError>(true);

        public static implicit operator Status<TError>(SuccessTag tag)
        {
            return SuccessStatus;
        }

        public static implicit operator Status<TError>(ErrorTag<TError> tag)
        {
            return new Status<TError>(tag.Error);
        }
    }
}
