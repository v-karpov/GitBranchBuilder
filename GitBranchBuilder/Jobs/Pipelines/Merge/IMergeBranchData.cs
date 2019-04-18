using System.Collections.Generic;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public interface IMergeBranchData
    {
        IEnumerable<BranchInfo> BranchesToMerge { get; }

        BranchInfo TargetBranch { get; }
    }
}