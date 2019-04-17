using NGitLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchBuilder
{
    public interface IBranchDataSource
    {
        IEnumerable<Branch> GetBranches(string projectId);
    }
}
