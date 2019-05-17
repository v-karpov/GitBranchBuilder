using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Делегат, описывающий конфигуратор результата выполнения конвейера
    /// </summary>
    public delegate Task<PipelineResult> ResultConfigurator<TJob>(IConfigurablePipeline<TJob> pipeline)
        where TJob : IJob;
    
    /// <summary>
    /// Интерфейс конфигуратора конвейера
    /// </summary>
    /// <typeparam name="TJob">Тип работ, выполняемых на ковейере</typeparam>
    public interface IPipelineConfigurator<TJob>
        where TJob : IJob
    {
        /// <summary>
        /// Список всех этапов конвейера
        /// </summary>
        IReadOnlyCollection<TJob> JobCollection { get; }

        /// <summary>
        /// Работа, являющаяся началом конвейера
        /// </summary>
        IStartJob Start { get; }

        /// <summary>
        /// Действие конфигурации, выполняемое при настройке конвейера
        /// </summary>
        ResultConfigurator<TJob> ConfigureResult { get; }
    }
}
