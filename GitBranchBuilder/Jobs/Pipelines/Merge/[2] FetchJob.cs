using System;
using System.Collections.Generic;
using System.Linq;

using LibGit2Sharp;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class FetchJob : Job, IFetchJob
    {
        public override string Description => $"Fetching from {Remote.Name}...";

        public Repository Repository { get; protected set; }

        public Remote Remote { get; protected set; }

        public IEnumerable<string> RefSpecs { get; protected set; }

        public FetchJob(IRepositoryProvider repositoryProvider, IRemoteProvider remoteProvider, ICredentialsProvider credentialsProvider)
            : base()
        {
            Repository = repositoryProvider.Repository;

            Prepare = () =>
            {
                Remote = remoteProvider.Remote;
                RefSpecs = Remote.FetchRefSpecs.Select(x => x.Specification);
            };
         
            Process = () =>
            {
                Commands.Fetch(Repository, Remote.Name, RefSpecs,
                    new FetchOptions
                    {
                        CredentialsProvider = credentialsProvider.GetCredentials,
                        OnProgress = str => { Console.WriteLine(str); return true; }
                    }, string.Empty);
            };
        }
    }
}
