using System;
using System.Collections.Generic;
using System.Linq;

using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class LocalBranchProvider : RepositoryHolder, ILocalBranchProvider
    {
        public Dictionary<string, Branch> Branches => BranchesLoader.Value;

        public Lazy<Dictionary<string, Branch>> BranchesLoader { get; }

        public Func<string, Branch> CreateBranch { get; }

        public Branch GetBranch(BranchInfo branch)
            => Branches.TryGetValue(branch.Name, out var result) 
                ? result
                : CreateBranch(branch.Name);

        public LocalBranchProvider(
            IRepositoryProvider repositoryProvider,
            IDefaultBranch defaultBranch) : base (repositoryProvider)
        {
            BranchesLoader = new Lazy<Dictionary<string, Branch>>(() => Repository.Branches
                .Where(x => !x.IsRemote)
                .ToDictionary(x => x.FriendlyName));

            CreateBranch = branchName =>
                Repository.CreateBranch(branchName, defaultBranch.Branch.Tip);
        }
    }
}
