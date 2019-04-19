using System;
using System.Linq;
using System.Threading.Tasks;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class MergePipeline : IJobPipeline
    {
        public bool ForceParallel => true;

        public Func<Task> ExecuteAction { get; }

        public void Dispose()
        {
            // TODO
        }

        private Task PerformJob(IJob job)
        {
            job.Task.Start();
            return job.Task;
        }

        private Task StartTaskIfNeeded(Task task)
        {
            if (task.Status == TaskStatus.Created)
            {
                task.Start();
            }

            return task;
        }

        private Task PerformConcurrently(params IJob[] jobs)
        {
            var tasks = jobs
                .Select(x => x.Task)
                .Select(StartTaskIfNeeded);

            return Task.WhenAll(tasks);
        }

        private async Task PerformSequentially(params IJob[] jobs)
        {
            foreach (var job in jobs)
            {
                await StartTaskIfNeeded(job.Task);
            }
        }

        public MergePipeline(
            FetchJob fetch,
            PrepareBranchJob prepareBranch,
            MergeJob merge,
            BuildJob build,
            PushJob push)
        {
            ExecuteAction = async () =>
            {
                await PerformConcurrently(fetch, prepareBranch);
                await PerformSequentially(merge, build, push);
            };
        }
    }
}
