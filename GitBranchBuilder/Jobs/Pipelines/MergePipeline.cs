using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class MergePipeline : IJobPipeline
    {
        public IEnumerable<IJob> Jobs { get; }

        public bool ForceParallel => true;

        public void Dispose()
        {
            // TODO
        }

        public MergePipeline(
            IFetchJob fetchJob,
            PrepareBranchJob prepareBranchJob,
            IMergeJob mergeJob,
            BuildJob buildJob,
            PushJob pushJob)
        {
            Jobs = new IJob[] { prepareBranchJob, fetchJob, mergeJob, buildJob, pushJob };
        }
    }
}
