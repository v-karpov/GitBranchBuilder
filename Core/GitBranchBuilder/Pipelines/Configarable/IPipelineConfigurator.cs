using System.Threading.Tasks.Dataflow;

using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Pipelines.Configarable
{
    /// <summary>
    /// Делегат, описывающий конфигуратор результата выполнения конвейера
    /// </summary>
    public delegate ISourceBlock<TResult> ResultConfigurator<TIn, TResult>(IConfigurablePipeline<TIn, TResult> pipeline)
        where TResult : IResult;

    /// <summary>
    /// Интерфейс конфигуратора конвейера
    /// </summary>
    /// <typeparam name="TJob">Тип работ, выполняемых на ковейере</typeparam>
    public interface IPipelineConfigurator<TIn, TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Начальный блок конвейера
        /// </summary>
        ITargetBlock<TIn> Start { get; }

        /// <summary>
        /// Действие конфигурации, выполняемое при настройке конвейера
        /// </summary>
        ResultConfigurator<TIn, TResult> ConfigureResult { get; }
    }
}
