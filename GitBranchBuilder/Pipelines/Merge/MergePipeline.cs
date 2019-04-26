using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines.Merge
{
    public interface IMergeJob : IJob { }

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
                Jobs = CreateCollection<IMergeJob>(prepareBranch, fetch, merge, build, push);
                Start = new CombinedStartJob(CreateCollection<IStartJob>(prepareBranch, fetch));

                ConfigureResult = pipeline =>
                    Join(prepareBranch, fetch, pipeline, result => result.Item1)
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
