using System;
using LibGit2Sharp;

namespace GitBranchBuilder.Core
{
    public class OriginRemoteProvdier : RepositoryHolder, IRemoteProvider
    {
        public const string RemoteName = "origin";

        public Lazy<Remote> Remote { get; }

        public OriginRemoteProvdier(IRepositoryProvider repositoryProvider) : base(repositoryProvider)
            => Remote = new Lazy<Remote>(() => Repository.Network.Remotes[RemoteName]);
    }
}
