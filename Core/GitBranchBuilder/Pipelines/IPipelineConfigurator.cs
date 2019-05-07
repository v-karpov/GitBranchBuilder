using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines
{
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
        Func<IConfigurablePipeline<TJob>, Task<Result>> ConfigureResult { get; }
    }
}
