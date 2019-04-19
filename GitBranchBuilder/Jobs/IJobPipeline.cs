using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Дейсвтие, выполняемое данным конвейером
        /// </summary>
        Func<Task> ExecuteAction { get; }
    }
}
