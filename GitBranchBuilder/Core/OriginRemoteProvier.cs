using LibGit2Sharp;

namespace GitBranchBuilder.Core
{
    public class OriginRemoteProvdier : RepositoryHolder, IRemoteProvider
    {
        public const string RemoteName = "origin";

        public Remote Remote { get; }

        public OriginRemoteProvdier(IRepositoryProvider repositoryProvider) : base(repositoryProvider)
            => Remote = Repository.Network.Remotes[RemoteName];
    }
}
