using System.Threading.Tasks.Dataflow;
using GitBranchBuilder.Jobs;
using SharpConfig;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PrepareBranchJob : StartJob<MergeBranchData>, IMergeStartJob
    {
        public override string Description
            => $"Collecting branch data for {ProjectName}";

        public Configuration Configuration { get; }

        public string ProjectName { get; protected set; }
                
        public PrepareBranchJob(
            IBranchSource branchSource,
            IBranchCombiner branchCombiner,
            IConfigurationProvider configurationProvider,
            FetchJob fetch, MergeJob merge)
        {
            Configuration = configurationProvider.Configuration;

            Prepare = options =>
            {
                ProjectName = Configuration["GitLab"]["Project"].StringValue;
            };

            Process = () =>
            {
                var branchesToMerge = branchSource.GetBranches(ProjectName);

                return new MergeBranchData
                {
                    BranchesToMerge = branchesToMerge,
                    TargetBranch = branchCombiner.Combine(branchesToMerge),
                };
            };

            // соединение блоков
            var joint = new JoinBlock<MergeBranchData, string>();
            var transfomer = new TransformBlock<System.Tuple<MergeBranchData, string>, MergeBranchData>(x => x.Item1);

            this.LinkTo(joint.Target1);
            fetch.LinkTo(joint.Target2);

            joint.LinkTo(transfomer);
            transfomer.LinkTo(merge);
        }
    }
}
