using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Конвейер, настраиваемый при помощи <see cref="IConfigurablePipeline{TJob}"/>
    /// </summary>
    /// <typeparam name="TJob">Тип работы, используемый в данном конвейере</typeparam>
    public abstract class ConfigurablePipeline<TJob> : Pipeline, IConfigurablePipeline<TJob>
        where TJob : IJob
    {
        /// <summary>
        /// Конфигуратор конвейера
        /// </summary>
        public IPipelineConfigurator<TJob> Configurator { get; }

        /// <summary>
        /// Запускает задание на конвейере с заданными опциями 
        /// </summary>
        /// <param name="options">Опции запуска задания</param>
        /// <returns></returns>
        public override Task<Result> Run(StartOptions options)
        {
            var result = Configurator.ConfigureResult(this);

            Configurator.Start.Post(options);
            return result;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="pipelineConfigurator"></param>
        public ConfigurablePipeline(IPipelineConfigurator<TJob> pipelineConfigurator)
        {
            Configurator = pipelineConfigurator;
        }
    }
}
