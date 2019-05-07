using System.Collections.Generic;
using GitBranchBuilder.Components;
using GitBranchBuilder.Jobs;
using NGitLab.Models;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PrepareBranchJob : StartJob<MergeBranchData>, IMergeJob
    {
        public override string Description
            => $"Collecting branch data for {Project.Value.Name}";

        public Holder<Project> Project { get; set; }
                
        public PrepareBranchJob(
            Holder<List<BranchInfo>> branches,
            IBranchCombiner branchCombiner)
        {
            Process = () => new MergeBranchData
            {
                BranchesToMerge = branches,
                TargetBranch = branchCombiner.Combine(branches.Value),
            };
        }
    }
}
