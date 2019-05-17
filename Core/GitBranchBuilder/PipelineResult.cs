using CSharpFunctionalExtensions;

namespace GitBranchBuilder
{
    /// <summary>
    /// Абстрактный класс, описывающий результат работы конвейера
    /// </summary>
    public abstract class PipelineResult : IResult
    {
        internal class UnknownResult : PipelineResult
        {
            public override bool IsFailure => false;

            public override bool IsSuccess => false;
        }

        /// <summary>
        /// Результат выполнения неизвестен
        /// </summary>
        public static PipelineResult Unknown { get; } = new UnknownResult();

        public abstract bool IsFailure { get; }
    
        public abstract bool IsSuccess { get; }
    }
}
