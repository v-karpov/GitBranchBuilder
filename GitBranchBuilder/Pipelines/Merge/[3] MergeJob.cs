using System;
using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Jobs;

using LibGit2Sharp;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class MergeJob : PropagationJob<MergeBranchData, string>
    {
        public override string Description => $"Merging actual branches into {TargetBranch.FriendlyName}";

        public Branch TargetBranch { get; protected set; }

        public IEnumerable<Branch> BranchesToMerge { get; protected set; }

        public MergeJob(
            IRepositoryProvider repositoryProvider,
            IRemoteBranchProvider remoteBranches,
            ILocalBranchProvider localBranches,
            BuildJob build)
        {
            var repo = repositoryProvider.Repository;

            Prepare = input =>
            {
                BranchesToMerge = input.BranchesToMerge.Select(remoteBranches.GetBranch);
                TargetBranch = localBranches.GetBranch(input.TargetBranch);

                Commands.Checkout(repo, TargetBranch);
            };

            Process = () =>
            {
                var mergeSignature = new Signature("parovoz", "parovoz@elewise.com", DateTimeOffset.Now);

                foreach (var sourceBranch in BranchesToMerge)
                {
                    var result = repo.Merge(sourceBranch, mergeSignature);

                    if (result.Status == MergeStatus.Conflicts)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Unable to merge {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName} automatically");
                        Console.WriteLine($"INFO: {repo.Index.Conflicts.Count()} conflict(s) found. Resolve all the conflicts and press any key to continue...");

                        Console.ReadKey();
                    }
                }

                return "OK";
            };

            LinkTo(build);
        }
    }
}
