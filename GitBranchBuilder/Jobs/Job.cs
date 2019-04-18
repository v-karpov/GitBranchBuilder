using System;

namespace GitBranchBuilder.Jobs
{
    public abstract class Job : IJob
    {
        public virtual bool IsThreadsafe => false;

        public virtual string Description => $"Do {GetType().Name}";

        public Action Process { get; protected set; }

        public Action Prepare { get; protected set; }

        protected virtual void PrepareJob() { }

        protected virtual void ProcessJob() { }

        public virtual void Dispose() { }

        public Job()
        {
            Prepare = PrepareJob;
            Process = ProcessJob;
        }
    }
}
