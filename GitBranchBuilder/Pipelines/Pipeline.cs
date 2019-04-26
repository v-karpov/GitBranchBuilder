using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Базовый класс конвейера задач
    /// </summary>
    public abstract class Pipeline : IPipeline
    {
        /// <summary>
        /// Опции выполнения задач обработки
        /// </summary>
        public virtual ExecutionDataflowBlockOptions ExecutionOptions { get; }
            = new ExecutionDataflowBlockOptions();

        /// <summary>
        /// Опции группировки в задачах
        /// </summary>
        public virtual GroupingDataflowBlockOptions GroupingOptions { get; }
            = new GroupingDataflowBlockOptions();

        /// <summary>
        /// Опции связи задач в конвейере
        /// </summary>
        public virtual DataflowLinkOptions LinkOptions { get; }
            = new DataflowLinkOptions();

        /// <summary>
        /// Очищает ресурсы конвейера и его зависимостей
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Запускает задание на конвейере с заданными опциями 
        /// </summary>
        /// <param name="options">Опции запуска задания</param>
        /// <returns></returns>
        public abstract Task<PipelineResult> Run(PipelineOptions options);
    }
}
