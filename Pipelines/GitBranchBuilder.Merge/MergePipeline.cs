using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines.Merge
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMergeJob : IJob
    {

    }

    /// <summary>
    /// Конвейер, выполняющий слияние веток
    /// </summary>
    public class MergePipeline : ConfigurablePipeline<IMergeJob>
    {
        /// <summary>
        /// Конфигуратор для конвейера <see cref="MergePipeline"/>
        /// </summary>
        private class PipelineConfigurator : PipelineConfigurator<IMergeJob>
        {
            public PipelineConfigurator(
                PrepareBranchJob prepareBranch,
                FetchJob fetch,
                MergeJob merge,
                BuildJob build,
                PushJob push)
            {
                JobCollection = FromJobs(prepareBranch, fetch, merge, build, push);
                Start = new CombinedStartJob(FromCollection<IStartJob>(prepareBranch, fetch));

                ConfigureResult = pipeline => 
                    pipeline.Join(prepareBranch, fetch, x => x.Item1)
                        .LinkTo(merge)
                        .LinkTo(build)
                        .LinkTo(push)
                        .Result;
            }
        }
        
        public MergePipeline(IPipelineConfigurator<IMergeJob> configurator)
            : base(configurator)
        {

        }
    }
}
