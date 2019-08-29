using System;
using CSharpFunctionalExtensions;
using GitBranchBuilder.Components;
using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Providers;

using LibGit2Sharp;

using Microsoft.Build.Framework;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class PushJob : RetryJob<Result<BuildEngineResult>>
    {
        public class PushJobImpl : TrialJob<Result<BuildEngineResult>>
        {
            public override string Description
                => $"Pushing result into {RemoteName}/{Head.FriendlyName}";

            public string RemoteName { get; set; }

            public Branch Head { get; set; }

            public PushJobImpl(
                RepositoryHolder repository,
                Holder<Remote> remote,
                ICredentialsProvider credentialsProvider,
                Holder<DefaultBranchInfo> defaultBranch)
            {
                Prepare = buildResult =>
                {
                    if (!buildResult.IsSuccess)
                    {
                        throw Fatal(new InvalidOperationException("No way to push unchecked branch."));
                    }

                    RemoteName = remote.Value.Name;
                    Head = repository.Value.Head;
                };

                Process = () =>
                {
                    if (Head == defaultBranch.Value.Branch)
                    {
                        throw Fatal(new InvalidOperationException("Unable to push into default protected branch"));
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

                    return Result.Ok();
                };
            }
        }
    }
}
