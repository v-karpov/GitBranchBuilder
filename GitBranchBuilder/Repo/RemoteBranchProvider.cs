using System;
using System.Linq;

using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class RemoteBranchProvider : RepositoryHolder, IRemoteBranchProvider
    {
        public Remote Remote => RemoteLoader.Value;

        public Lazy<Remote> RemoteLoader { get; }

        public ILookup<string, Branch> Branches => BranchesLoader.Value;

        public Lazy<ILookup<string, Branch>> BranchesLoader { get; private set; }

        public Branch GetBranch(BranchInfo branch)
            => Branches[branch.Name].FirstOrDefault();

        public RemoteBranchProvider(IRepositoryProvider repositoryProvider, IRemoteProvider remoteProvider)
            : base(repositoryProvider)
        {
            RemoteLoader = remoteProvider.Remote;

            BranchesLoader = new Lazy<ILookup<string, Branch>>(() => Repository.Branches
                .Where(x => x.IsRemote)
                .ToLookup(x => x.FriendlyName.Replace(Remote.Name + "/", "")));
        }
    }
}
