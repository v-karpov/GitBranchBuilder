using CSharpFunctionalExtensions;

namespace GitBranchBuilder
{
    public struct ResultWrapper<T> : IResult
    {
        public bool IsFailure => wrappedResult.IsFailure;

        public bool IsSuccess => wrappedResult.IsSuccess;

        public T Value => wrappedResult.IsSuccess
            ? wrappedResult.Value
            : initialValue;

        private readonly T initialValue;
        private readonly Result<T> wrappedResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="wrappedResult"></param>
        internal ResultWrapper(T initialValue, Result<T> wrappedResult)
        {
            this.initialValue = initialValue;
            this.wrappedResult = wrappedResult;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ResultWrapper
    {
        public static ResultWrapper<T> Ok<T>(T result)
            => new ResultWrapper<T>(result, Result.Ok(result));

        public static ResultWrapper<T> Fail<T>(T initialValue, string error)
           => new ResultWrapper<T>(initialValue, Result.Fail<T>(error));
    }
}
