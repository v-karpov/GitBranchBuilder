using System.Collections.Generic;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class MergeBranchData : IMergeBranchData
    {
        public IEnumerable<BranchInfo> BranchesToMerge { get; internal set; }

        public BranchInfo TargetBranch { get; internal set; }
    }
}
