using System;
using System.Collections.Generic;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Поставщик задач для выполнения в рамках одного конвейера задач
    /// </summary>
    public interface IJobPipeline : IDisposable
    {
        /// <summary>
        /// Выполнять данный конвейер задач параллельно с остальными
        /// </summary>
        bool ForceParallel { get; }

        /// <summary>
        /// Задачи, выполняемые на данном конвейере
        /// </summary>
        IEnumerable<IJob> Jobs { get; }
    }
}
