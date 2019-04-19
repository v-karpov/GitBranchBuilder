using System;
using System.Threading.Tasks;

namespace GitBranchBuilder.Jobs
{
    public abstract class Job : Task, IJob
    {
        public virtual string Description => $"Do {GetType().Name}";

        public Task Task => this;

        protected Action Process { get; set; }

        protected Action Prepare { get; set; }
       
        protected virtual void Execute()
        {
            Prepare?.Invoke();

            Console.WriteLine($"{Description}");

            Process?.Invoke();
        }
        private Job(Action target) : base(() => target())
        {
            target = Execute;
        }

        public Job() : this(null)
        {
            
        }
    }

    public abstract class Job<TResult> : Task<TResult>, IJob
    {
        public virtual string Description => $"Do {GetType().Name}";

        public Func<TResult> Process { get; protected set; }

        public Action Prepare { get; protected set; }

        public Task Task => this;

        protected virtual TResult Execute()
        {
            Prepare?.Invoke();

            Console.WriteLine($"{Description}");

            return Process.Invoke();
        }
        private Job(Func<TResult> target) : base(() => target())
        {
            target = Execute;
        }

        public Job() : this(null)
        {

        }
    }
}
