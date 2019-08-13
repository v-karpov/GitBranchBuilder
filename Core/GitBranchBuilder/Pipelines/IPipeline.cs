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
        Task Run();

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

    /// <summary>
    /// Интерфейс, описывающий последовательность работ
    /// </summary>
    /// <typeparam name="TIn">Тип входных данных конвейера</typeparam>
    /// <typeparam name="TResult">Тип результата конвейера</typeparam>
    public interface IPipeline<TIn, TResult> : IPipeline
    {
        /// <summary>
        /// Выполняет работу в конвейере до конца
        /// </summary>
        Task<TResult> Run(TIn options);
    }
}
