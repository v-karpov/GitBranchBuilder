using System;
using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Components;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Providers;

using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class MergeJob : PropagationJob<MergeBranchData, string>, IMergeJob
    {
        public override string Description => $"Merging actual branches into {TargetBranch.FriendlyName}";

        public Branch TargetBranch { get; protected set; }

        public IEnumerable<Branch> BranchesToMerge { get; protected set; }

        public MergeJob(RepositoryHolder repository,
                        ConfigurationHolder configuration,
                        IRemoteBranchProvider remoteBranches,
                        ILocalBranchProvider localBranches)
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
                    var result = repository.Value.Merge(sourceBranch, mergeSignature, mergeOptions);

                    Console.WriteLine();

                    if (result.Status == MergeStatus.Conflicts)
                    {
                        Console.WriteLine($"Unable to merge {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName} automatically");
                        Console.WriteLine($"INFO: {repository.Index.Conflicts.Count()} conflict(s) found. Resolve all the conflicts and press any key to continue...");

                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"Merged successfully {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName}");
                    }
                }

                return "OK";
            };
        }
    }
}
