namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Опции запуска конвейера работ
    /// </summary>
    public class PipelineOptions
    {

        /// <summary>
        /// Требуется ли запускать все возможные участки в параллельном режиме
        /// </summary>
        public bool ForceParallel { get; set; }
    }
}
