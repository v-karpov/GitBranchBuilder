using System.Linq;
using LibGit2Sharp;

namespace GitBranchBuilder
{
    public interface IBranchProvider
    {
        Branch GetBranch(BranchInfo branch);
    }

    public interface ILocalBranchProvider : IBranchProvider
    {

    }

    public interface IRemoteBranchProvider : IBranchProvider
    {
        Remote Remote { get; }
    }


    public interface IDefaultBranch
    {
        Branch Branch { get; }
    }
}
