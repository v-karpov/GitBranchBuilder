using System;
using System.Collections.Generic;
using System.Linq;
using GitBranchBuilder.Jobs;
using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class FetchJob : StartJob<string>, IMergeStartJob
    {
        public override string Description => $"Fetching from {Remote.Name}...";

        public Repository Repository { get; set; }

        public Remote Remote { get; set; }

        public IEnumerable<string> RefSpecs { get; protected set; }

        public FetchJob(IRepositoryProvider repositoryProvider, IRemoteProvider remoteProvider, ICredentialsProvider credentialsProvider)
            : base()
        {
            Repository = repositoryProvider.Repository;
            Remote = remoteProvider.Remote;

            Prepare = options =>
            {
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

                return "OK";
            };
        }
    }
}
