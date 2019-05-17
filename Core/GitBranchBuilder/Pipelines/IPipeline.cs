using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Интерфейс, описывающий последовательность работ
    /// </summary>
    public interface IPipeline : IDisposable
    {
        /// <summary>
        /// Выполняет работу в конвейере до конца
        /// </summary>
        Task<PipelineResult> Run(StartOptions options);

        /// <summary>
        /// Глобальные параметры обрабатывающих блоков
        /// </summary>
        ExecutionDataflowBlockOptions ExecutionOptions { get; }

        /// <summary>
        /// Глобальные параметры группирующих блоков
        /// </summary>
        GroupingDataflowBlockOptions GroupingOptions { get; }

        /// <summary>
        /// Глобальные параметры связи блоков в конвейере
        /// </summary>
        DataflowLinkOptions LinkOptions { get; }
    }
}
