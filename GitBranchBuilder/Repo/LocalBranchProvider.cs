using System.Collections.Generic;
using System.Linq;

using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class LocalBranchProvider : ILocalBranchProvider
    {
        public Repository Repository { get; }
        
        public Branch DefaultBranch { get; }

        public Dictionary<string, Branch> Branches { get; }

        public Branch GetBranch(BranchInfo branch)
            => Branches.TryGetValue(branch.Name, out var result) 
                ? result
                : Repository.CreateBranch(branch.Name, DefaultBranch.Tip);

        public LocalBranchProvider(
            IRepositoryProvider repositoryProvider,
            IDefaultBranch defaultBranch)
        {
            Repository = repositoryProvider.Repository;
            
            Branches = Repository.Branches
                .Where(x => !x.IsRemote)
                .ToDictionary(x => x.FriendlyName);

            DefaultBranch = defaultBranch.Branch;
        }
    }
}
