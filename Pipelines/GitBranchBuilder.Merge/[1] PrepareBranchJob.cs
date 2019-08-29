using System.Collections.Generic;

using CSharpFunctionalExtensions;

using GitBranchBuilder.Components;
using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Jobs;

using NGitLab.Models;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PrepareBranchJob : PropagationJob<Result, MergeBranchData>
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
