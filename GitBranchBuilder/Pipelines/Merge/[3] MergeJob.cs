using System;
using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Jobs;

using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class MergeJob : PropagationJob<MergeBranchData, string>, IMergeJob
    {
        public override string Description => $"Merging actual branches into {TargetBranch.FriendlyName}";

        public Branch TargetBranch { get; protected set; }

        public IEnumerable<Branch> BranchesToMerge { get; protected set; }

        public MergeJob(
            IRepositoryProvider repositoryProvider,
            IRemoteBranchProvider remoteBranches,
            ILocalBranchProvider localBranches,
            IConfigurationProvider configurationProvider)
        {
            var repo = repositoryProvider.Repository;
            var mergeConfig = configurationProvider.Configuration["Merge"];
            
            Prepare = input =>
            {
                BranchesToMerge = input.BranchesToMerge.Select(remoteBranches.GetBranch);
                TargetBranch = localBranches.GetBranch(input.TargetBranch);

                Commands.Checkout(repo, TargetBranch);
            };

            Process = () =>
            {
                var mergeSignature = new Signature("parovoz", "parovoz@elewise.com", DateTimeOffset.Now);
                var mergeOptions = new MergeOptions
                {
                    MergeFileFavor = Enum.TryParse(mergeConfig["FileFavor"].StringValue, out MergeFileFavor favor) 
                        ? favor
                        : MergeFileFavor.Normal,
                    SkipReuc = true
                };

                foreach (var sourceBranch in BranchesToMerge)
                {
                    var result = repo.Merge(sourceBranch, mergeSignature, mergeOptions);

                    Console.WriteLine();

                    if (result.Status == MergeStatus.Conflicts)
                    {
                        Console.WriteLine($"Unable to merge {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName} automatically");
                        Console.WriteLine($"INFO: {repo.Index.Conflicts.Count()} conflict(s) found. Resolve all the conflicts and press any key to continue...");

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
