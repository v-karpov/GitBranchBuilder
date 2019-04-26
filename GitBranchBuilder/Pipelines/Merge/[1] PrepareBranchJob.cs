using GitBranchBuilder.Jobs;

using SharpConfig;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PrepareBranchJob : StartJob<MergeBranchData>, IMergeJob
    {
        public override string Description
            => $"Collecting branch data for {ProjectName}";

        public Configuration Configuration { get; }

        public string ProjectName { get; protected set; }
                
        public PrepareBranchJob(
            IBranchSource branchSource,
            IBranchCombiner branchCombiner,
            IConfigurationProvider configurationProvider)
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
        }
    }
}
