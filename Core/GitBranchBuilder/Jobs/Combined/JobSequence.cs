using System.Collections;
using System.Collections.Generic;

namespace GitBranchBuilder.Jobs.Combined
{
    public class JobSequence<TIn, TOut> : PropagationJob<TIn, TOut>, IEnumerable<IJob>
    {
        public virtual IReadOnlyList<IJob> Jobs { get; protected set; }

        public IEnumerator<IJob> GetEnumerator() => Jobs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
