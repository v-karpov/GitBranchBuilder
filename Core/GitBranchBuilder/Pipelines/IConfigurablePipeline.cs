using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Интерфейс конфигурируемого конвейера
    /// </summary>
    /// <typeparam name="TJob">Тип работ, выполняемых на данном конвейере</typeparam>
    public interface IConfigurablePipeline<TJob> : IPipeline
        where TJob : IJob
    {
        /// <summary>
        /// Конфигуратор конвейера
        /// </summary>
        IPipelineConfigurator<TJob> Configurator { get; }
    }
}
