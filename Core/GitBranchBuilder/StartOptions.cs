namespace GitBranchBuilder
{
    /// <summary>
    /// Опции запуска конвейера работ
    /// </summary>
    public class StartOptions
    {

        /// <summary>
        /// Требуется ли запускать все возможные участки в параллельном режиме
        /// </summary>
        public bool ForceParallel { get; set; }
    }
}
