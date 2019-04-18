using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitBranchBuilder.Repo;
using LibGit2Sharp;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public class MergeJob : Job, IMergeJob
    {
        public override string Description => $"Merging actual branches into {TargetBranch.FriendlyName}";

        public Branch TargetBranch { get; protected set; }

        public IEnumerable<Branch> BranchesToMerge { get; protected set; }

        public MergeJob(
            IRepositoryProvider repositoryProvider,
            IRemoteBranchProvider remoteBranches,
            ILocalBranchProvider localBranches,
            IMergeBranchData branchData)
        {
            var repo = repositoryProvider.Repository;

            Prepare = () =>
            {
                BranchesToMerge = branchData.BranchesToMerge
                    .Select(remoteBranches.GetBranch);

                TargetBranch = localBranches.GetBranch(branchData.TargetBranch);

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
                        Console.WriteLine($"Unable to merge {sourceBranch.FriendlyName} into {TargetBranch.FriendlyName}. \r\n INFO: {repo.Index.Conflicts.Count()} conflict(s) found. Resolve all the conflicts and press any key to continue...");
                        Console.ReadKey();
                    }
                }
            };
        }
    }
}
