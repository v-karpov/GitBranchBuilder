using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Pipelines.Configarable
{
    /// <summary>
    /// Интерфейс конфигурируемого конвейера
    /// </summary>
    /// <typeparam name="TJob">Тип работ, выполняемых на данном конвейере</typeparam>
    public interface IConfigurablePipeline<TIn, TResult> : IPipeline<TIn, TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Конфигуратор конвейера
        /// </summary>
        IPipelineConfigurator<TIn, TResult> Configurator { get; }
    }
}
