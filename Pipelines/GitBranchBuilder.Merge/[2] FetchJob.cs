using System;
using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Components;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Providers;

using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class FetchJob : StartJob<string>, IMergeJob
    {
        public override string Description => $"Fetching from {RemoteName}...";

        public IEnumerable<string> RefSpecs { get; protected set; }
       
        public string RemoteName { get; set; }

        public FetchJob(
            RepositoryHolder repositoryHolder,
            Holder<Remote> remoteHolder,
            ICredentialsProvider credentialsProvider)
        {
            Prepare = options =>
            {
                RemoteName = remoteHolder.Value.Name;
                RefSpecs = remoteHolder.Value.FetchRefSpecs.Select(x => x.Specification);
            };

            Process = () =>
            {
                Commands.Fetch(repositoryHolder, RemoteName, RefSpecs,
                    options: new FetchOptions
                    {
                        CredentialsProvider = credentialsProvider.GetValue,
                        OnProgress = str => { Console.WriteLine(str); return true; }
                    }, 
                    logMessage: string.Empty);

                return "OK";
            };
        }
    }
}
