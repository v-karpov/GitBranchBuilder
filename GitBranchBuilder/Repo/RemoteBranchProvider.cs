using System.Linq;

using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class RemoteBranchProvider : IRemoteBranchProvider
    {
        public Remote Remote { get; }

        public Repository Repository { get; }
        
        public ILookup<string, Branch> Branches { get; }

        public Branch GetBranch(BranchInfo branch)
            => Branches[branch.Name].FirstOrDefault();

        public RemoteBranchProvider(IRepositoryProvider repositoryProvider, IRemoteProvider remoteProvider)
        {
            Remote = remoteProvider.Remote;
            Repository = repositoryProvider.Repository;

            Branches = Repository.Branches
                .Where(x => x.IsRemote)
                .ToLookup(x => x.FriendlyName.Replace(Remote.Name + "/", ""));
        }
    }
}
