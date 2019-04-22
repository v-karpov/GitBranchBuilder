using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Autofac;

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
        Task<PipelineResult> Run(PipelineOptions options);

        /// <summary>
        /// Метод, выполняющий подготовку с использованием контейнера зависимостей <see cref="Autofac.IContainer"/>
        /// </summary>
        /// <param name="container"></param>
        void Prepare(IContainer container);

        /// <summary>
        /// Параметры обрабатывающих блоков
        /// </summary>
        ExecutionDataflowBlockOptions ExecutionOptions { get; }

        /// <summary>
        /// Параметры группирующих блоков
        /// </summary>
        GroupingDataflowBlockOptions GroupingOptions { get; }
    }
}
