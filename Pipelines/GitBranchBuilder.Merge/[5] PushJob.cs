using System;

using GitBranchBuilder.Components;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Providers;

using LibGit2Sharp;

using Microsoft.Build.Framework;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PushJob : FinalJob<BuildEngineResult>, IMergeJob
    {
        public override string Description 
            => $"Pushing result into {RemoteName}/{Head.FriendlyName}";

        public string RemoteName { get; set; }

        public Branch Head { get; set; }

        public PushJob(
            RepositoryHolder repository,
            Holder<Remote> remote,
            ICredentialsProvider credentialsProvider,
            Holder<DefaultBranchInfo> defaultBranch)
        {
            Prepare = buildResult =>
            {
                if (!buildResult.Result)
                {
                    Fault(new InvalidOperationException("No way to push unchecked branch."));
                    return;
                }

                RemoteName = remote.Value.Name;
                Head = repository.Value.Head;
            };

            Process = () =>
            {
                if (Head == defaultBranch.Value.Branch)
                {
                    Fault(new InvalidOperationException("Unable to push into default protected branch"));
                    return GitBranchBuilder.Result.Unknown;
                }

                if (!Head.IsTracking)
                {
                    repository.Branches.Update(Head,
                        x => x.Remote = RemoteName,
                        x => x.UpstreamBranch = Head.CanonicalName);
                }

                repository.Network.Push(
                    branch: Head,
                    pushOptions: new PushOptions
                    {
                        CredentialsProvider = credentialsProvider.GetValue,
                        PackbuilderDegreeOfParallelism = Environment.ProcessorCount,
                        OnPushStatusError = err => Console.WriteLine(err)
                    });

                return GitBranchBuilder.Result.Unknown;
            };
        }
    }
}
