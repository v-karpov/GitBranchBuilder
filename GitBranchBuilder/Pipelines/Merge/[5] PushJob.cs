using System;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Pipelines.Merge.Data;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PushJob : FinalJob<BuildJobResult>, IMergeJob
    {
        public override string Description 
            => $"Pushing result into {Remote.Name}/{Repository.Head.FriendlyName}";

        public Repository Repository { get; }

        public Remote Remote => RemoteLoader.Value;

        public Lazy<Remote> RemoteLoader { get; }

        public PushJob(
            IRepositoryProvider repositoryProvider,
            IRemoteProvider remoteProvider,
            ICredentialsProvider credentialsProvider,
            IDefaultBranch defaultBranch) : base()
        {
            Repository = repositoryProvider.Repository;
            RemoteLoader = remoteProvider.Remote;

            Process = () =>
            {
                var head = Repository.Head;

                if (head == defaultBranch.Branch)
                {
                    throw new InvalidOperationException("Unable to push into default protected branch");
                }

                if (!head.IsTracking)
                {
                    Repository.Branches.Update(head,
                        x => x.Remote = Remote.Name,
                        x => x.UpstreamBranch = head.CanonicalName);
                }

                Repository.Network.Push(
                    branch: Repository.Head,
                    pushOptions: new PushOptions
                    {
                        CredentialsProvider = credentialsProvider.GetCredentials,
                        PackbuilderDegreeOfParallelism = Environment.ProcessorCount,
                        OnPushStatusError = err => Console.WriteLine(err)
                    });

                return PipelineResult.Empty;
            };
        }
    }
}
