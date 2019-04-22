namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Класс, описывающий результат работы конвейера
    /// </summary>
    public class PipelineResult
    {
        /// <summary>
        /// Результат выполнения по умолчанию
        /// </summary>
        public static PipelineResult Empty { get; } = new PipelineResult();
    }
}
