namespace OperationResult.Tags
{
    public struct SuccessTag { }

    public struct SuccessTag<TResult>
    {
        internal readonly TResult Value;

        internal SuccessTag(TResult result)
        {
            Value = result;
        }
    }
}
