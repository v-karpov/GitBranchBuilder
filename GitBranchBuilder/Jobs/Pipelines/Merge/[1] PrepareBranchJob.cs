using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpConfig;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class PrepareBranchJob : Job
    {
        public override string Description
            => $"Collecting branch data for {ProjectName}";

        public Configuration Configuration { get; }

        public string ProjectName { get; protected set; }
                
        public PrepareBranchJob(
            IBranchSource branchSource,
            IBranchCombiner branchCombiner,
            IConfigurationProvider configurationProvider,
            MergeBranchData branchData)
        {
            Configuration = configurationProvider.Configuration;

            Prepare = () =>
            {
                ProjectName = Configuration["GitLab"]["Project"].StringValue;
            };

            Process = () =>
            {
                branchData.BranchesToMerge = branchSource.GetBranches(ProjectName);
                branchData.TargetBranch = branchCombiner.Combine(branchData.BranchesToMerge);
            };
        }
    }
}
