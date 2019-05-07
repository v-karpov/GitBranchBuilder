namespace GitBranchBuilder
{
    /// <summary>
    /// Абстрактный класс, описывающий результат работы конвейера
    /// </summary>
    public abstract class Result
    {
        internal class UnknownResult : Result
        {

        }

        /// <summary>
        /// Результат выполнения не известен, либо не важен
        /// </summary>
        public static Result Unknown { get; } = new UnknownResult();
    }
}
