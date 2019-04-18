using System.Collections.Generic;

namespace GitBranchBuilder
{
    public interface IBranchSource
    {
        IEnumerable<BranchInfo> GetBranches(string projectName);
    }
}
