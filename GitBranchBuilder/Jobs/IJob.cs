using System;

namespace GitBranchBuilder.Jobs
{
    public interface IJob : IDisposable
    {
        /// <summary>
        /// Является ли выполнение задания потокобезопасным
        /// </summary>
        bool IsThreadsafe { get; }

        string Description { get; }

        Action Process { get; }

        Action Prepare { get; }
    }
}
