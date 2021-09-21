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

        internal static bool Equals<V1, V2>((bool, V1, V2) r1, (bool, V1, V2) r2)
        {
            if (r1.Item1 != r2.Item1)
            {
                return false;
            }
            if (r1.Item1) 
            {
                if (r1.Item2 == null)
                {
                    return r2.Item2 == null;
                }
                return r1.Item2.Equals(r2.Item2);
            }
            if (r1.Item3 == null)
            {
                return r2.Item3 == null;
            }
            return r1.Item3.Equals(r2.Item3);
        }
    }
}
