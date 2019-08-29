using System.Threading.Tasks.Dataflow;
using CSharpFunctionalExtensions;
using GitBranchBuilder.Components;
using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Pipelines.Configarable;
using GitBranchBuilder.Providers;
using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    /// <summary>
    /// Конвейер, выполняющий слияние веток
    /// </summary>
    public class MergePipeline : ConfigurablePipeline
    {
        public class FetchJob : FetchJob<Result> { }

        public class BuildJob : RetryBuildJob<string>
        {
            public class Impl : BuildJobImpl { }
        }

        /// <summary>
        /// Конфигуратор для конвейера <see cref="MergePipeline"/>
        /// </summary>
        private class Config : PipelineConfigurator
        {
            public Config(
                PrepareBranchJob prepareBranch,
                FetchJob fetch,
                MergeJob merge,
                BuildJob build,
                PushJob push)
            {
                Start = Broadcast(prepareBranch, fetch);

                ConfigureResult = pipeline =>
                   pipeline.Join(prepareBranch, fetch, x => x.Item1)
                        .LinkTo(merge)
                        .LinkTo(build)
                        .LinkTo(push);
            }
        }
    }
}
