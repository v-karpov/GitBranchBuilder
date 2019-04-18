using System;

using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class OriginRemoteProvdier : IRemoteProvider
    {
        public const string RemoteName = "origin";

        public Remote Remote => RemoteLoader.Value;

        protected Lazy<Remote> RemoteLoader { get; }

        protected Repository Repository { get; }

        public OriginRemoteProvdier(IRepositoryProvider repositoryProvider)
        {
            Repository = repositoryProvider.Repository;
            RemoteLoader = new Lazy<Remote>(() => Repository.Network.Remotes[RemoteName]);
        }
    }
}
