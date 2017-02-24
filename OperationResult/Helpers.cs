using OperationResult.Tags;

namespace OperationResult
{
    public static class Helpers
    {
        private static SuccessTag SuccessTag = new SuccessTag();

        /// <summary>
        /// Create "Success" Status or Result
        /// </summary>
        public static SuccessTag Ok()
        {
            return SuccessTag;
        }

        /// <summary>
        /// Create "Success" Status or Result
        /// </summary>
        public static SuccessTag<TResult> Ok<TResult>(TResult result)
        {
            return new SuccessTag<TResult>(result);
        }

        private static ErrorTag ErrorTag = new ErrorTag();

        /// <summary>
        /// Create "Error" Status or Result
        /// </summary>
        public static ErrorTag Error()
        {
            return ErrorTag;
        }

        /// <summary>
        /// Create "Error" Status or Result
        /// </summary>
        public static ErrorTag<TError> Error<TError>(TError error)
        {
            return new ErrorTag<TError>(error);
        }
    }
}
