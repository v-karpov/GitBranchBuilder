using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using CSharpFunctionalExtensions;

using GitBranchBuilder.Components.Holders;

namespace GitBranchBuilder.Pipelines.Configarable
{
    /// <summary>
    /// Конвейер, настраиваемый при помощи <see cref="IConfigurablePipeline{TJob}"/>
    /// </summary>
    /// <typeparam name="TJob">Тип работы, используемый в данном конвейере</typeparam>
    public abstract class ConfigurablePipeline : ConfigurablePipeline<Result>
    {

    }

    /// <summary>
    /// Конвейер, настраиваемый при помощи <see cref="IConfigurablePipeline{TJob}"/>
    /// </summary>
    /// <typeparam name="TJob">Тип работы, используемый в данном конвейере</typeparam>
    public abstract class ConfigurablePipeline<TIn> : ConfigurablePipeline<TIn, Result>
    {

    }

    /// <summary>
    /// Конвейер, настраиваемый при помощи <see cref="IConfigurablePipeline{TJob}"/>
    /// </summary>
    /// <typeparam name="TJob">Тип работы, используемый в данном конвейере</typeparam>
    /// <typeparam name="TResult">Тип результата работы, выполняемой на данном конвейере</typeparam>
    public abstract class ConfigurablePipeline<TIn, TResult> : Pipeline, IConfigurablePipeline<TIn, TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Конфигуратор конвейера
        /// </summary>
        public IPipelineConfigurator<TIn, TResult> Configurator { get; set; }

        /// <summary>
        /// Значение настроек, которое будет передано в метод 
        /// <see cref="Run(TIn)"/> при запуске
        /// </summary>
        public MaybeHolder<TIn> Input { get; set; }

        /// <summary>
        /// Запускает задание на конвейере с заданными опциями 
        /// </summary>
        /// <param name="options">Опции запуска задания</param>
        /// <returns></returns>
        public override Task Run()
            => Run(Input.Maybe.Unwrap());

        /// <summary>
        /// Запускает задание на конвейере с заданными опциями 
        /// </summary>
        /// <param name="options">Опции запуска задания</param>
        /// <returns></returns>
        public Task<TResult> Run(TIn options)
        {
            var resultBlock = Configurator.ConfigureResult(this);

            Configurator.Start.Post(options);
            return resultBlock.ReceiveAsync();
        }
    }
}
