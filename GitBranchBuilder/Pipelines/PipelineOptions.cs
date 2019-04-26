using Autofac;

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

        /// <summary>
        /// Контейнер зависимостей, используемый в системе для запуска задач
        /// </summary>
        public IContainer Container { get; internal set; }
    }
}
