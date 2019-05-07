using System.Collections.Generic;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class MergeBranchData
    {
        public List<BranchInfo> BranchesToMerge { get; internal set; }

        public BranchInfo TargetBranch { get; internal set; }
    }
}
