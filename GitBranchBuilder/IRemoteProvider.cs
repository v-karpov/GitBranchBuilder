using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitBranchBuilder
{
    public interface IRemoteProvider
    {
        Remote Remote { get; }
    }
}
