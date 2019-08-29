using System;
using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Components;
using GitBranchBuilder.Components.Holders.Specific;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Providers;

using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class MergeJob : PropagationJob<MergeBranchData, string>
    {
        public override string Description => $"Merging actual branches into {TargetBranch.FriendlyName}";

        public Branch TargetBranch { get; protected set; }

        public IEnumerable<Branch> BranchesToMerge { get; protected set; }

        public MergeJob(RepositoryHolder repository,
                        ConfigurationHolder configuration,
                        IRemoteBranchProvider remoteBranches,
                        ILocalBranchProvider localBranches,
                        IMergeApprovalService userApproval)
        {
            Prepare = data =>
            {
                BranchesToMerge = data.BranchesToMerge.Select(remoteBranches.GetBranch);
                TargetBranch = localBranches.GetBranch(data.TargetBranch);

                Commands.Checkout(repository.Value, TargetBranch);
            };

            Process = () =>
            {
                var mergeSignature = new Signature("parovoz", "parovoz@elewise.com", DateTimeOffset.Now);
                var mergeOptions = new MergeOptions
                {
                    MergeFileFavor = Enum.TryParse(configuration["Merge", "FileFavor"], out MergeFileFavor favor) 
                        ? favor
                        : MergeFileFavor.Normal,
                    SkipReuc = true
                };

                foreach (var sourceBranch in BranchesToMerge)
                {
                    var tip = repository.Head.Tip;
                    var result = repository.Value.Merge(sourceBranch, mergeSignature, mergeOptions);

                    if (result.Status == MergeStatus.Conflicts)
                    {
                        Log.Warn($"Unable to merge {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName} automatically");
                        Log.Error($"{repository.Index.Conflicts.Count()} conflict(s) found:");

                        foreach (var conflict in repository.Value.Index.Conflicts)
                        {
                            Log.Info(conflict.Ancestor.Path);
                        }

                        Log.Info("Resolve all the conflicts and press any key to continue...");
  
                        if (userApproval.RequstApprove("perform union merge").IsSuccess)
                        {
                            repository.Value.Reset(ResetMode.Hard, tip);
                            result = repository.Value.Merge(sourceBranch, mergeSignature, new MergeOptions
                            {
                                MergeFileFavor = MergeFileFavor.Union
                            });
                        }
                    }
                    else
                    {
                        Log.Info($"Merged successfully {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName}");
                    }
                }

                return "OK";
            };
        }
    }
}
