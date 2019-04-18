using System.Collections.Generic;
using LibGit2Sharp;

namespace GitBranchBuilder.Jobs
{
    public interface IMergeJob : IJob
    {
        IEnumerable<Branch> BranchesToMerge { get; }

        Branch TargetBranch { get; }
    }
}
