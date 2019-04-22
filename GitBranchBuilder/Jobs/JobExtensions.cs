using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    public static class JobExtensions
    {
        public static JobLink<T> LinkTo<T>(this ISourceBlock<T> job, IJob<T> target) 
            => new JobLink<T>(target, DataflowBlock.LinkTo(job, target));
    }
}
