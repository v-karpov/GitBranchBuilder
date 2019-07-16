using GitBranchBuilder.Jobs;
using GitBranchBuilder.Pipelines.Configarable;

namespace GitBranchBuilder.Pipelines.Merge
{
    /// <summary>
    /// Интерфейс работы в конвейере слияния веток
    /// </summary>
    public interface IMergeJob : IJob
    {

    }

    /// <summary>
    /// Конвейер, выполняющий слияние веток
    /// </summary>
    public class MergePipeline : ConfigurablePipeline<IMergeJob>
    {
        class FetchJob : FetchJob<StartOptions>, IStartJob, IMergeJob
        {
   
        }

        class BuildJob : RetryBuildJob<string>, IMergeJob
        {
            class Impl : BuildJobImpl { }
        }

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
                StartJob = new CombinedStartJob(FromCollection<IStartJob>(prepareBranch, fetch));

                ConfigureResult = pipeline => 
                    pipeline.Join(prepareBranch, fetch, x => x.Item1)
                        .LinkTo(merge)
                        .LinkTo(build)
                        .LinkTo(push)
                        .Result;
            }
        }
    }
}
