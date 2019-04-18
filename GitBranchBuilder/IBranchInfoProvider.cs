using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchBuilder
{
    public interface IBranchInfoProvider
    {
        BranchInfo GetBranchInfo(string branchName);
    }
}
