﻿using System;
using System.IO;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class PushJob : Job
    {
        public override string Description => $"Pushing result into {Remote.Name}/{Repository.Head.FriendlyName}";

        public Repository Repository { get; }

        public Remote Remote { get; }

        public CredentialsHandler CredentialsProvider { get; }

        public PushJob(
            IRepositoryProvider repositoryProvider,
            IRemoteProvider remoteProvider,
            ICredentialsProvider credentialsProvider,
            IDefaultBranch defaultBranch)
        {
            Repository = repositoryProvider.Repository;
            Remote = remoteProvider.Remote;
            CredentialsProvider = credentialsProvider.GetCredentials;

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
                        CredentialsProvider = CredentialsProvider,
                        PackbuilderDegreeOfParallelism = Environment.ProcessorCount,
                        OnPushStatusError = err => Console.WriteLine(err)
                    });
            };
        }
    }
}
