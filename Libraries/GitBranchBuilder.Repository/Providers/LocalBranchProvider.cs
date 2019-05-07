using System;
using System.Linq;

using GitBranchBuilder.Components;
using LibGit2Sharp;

using ILocalBranchLookup = System.Linq.ILookup<string, LibGit2Sharp.Branch>;

namespace GitBranchBuilder.Providers
{
    public interface ILocalBranchProvider : IBranchProvider
    {

    }

    public class LocalBranchProvider : ILocalBranchProvider
    {
        private class LocalBranchLookupProvider : Provider<ILocalBranchLookup>
        {
            public LocalBranchLookupProvider(
                RepositoryHolder repository)
            {
                ValueGetter = () =>
                    repository.Branches
                        .Where(x => !x.IsRemote)
                        .ToLookup(x => x.FriendlyName);
            }
        }

        public Holder<ILocalBranchLookup> Branches { get; }

        public Func<string, Branch> CreateBranch { get; }

        public Branch GetBranch(BranchInfo branch)
            => Branches.Value[branch.Name]
                .SingleOrDefault() ?? CreateBranch(branch.Name);

        public LocalBranchProvider(
            RepositoryHolder repository,
            Holder<DefaultBranchInfo> defaultBranch,
            Holder<ILocalBranchLookup> branchLookup)
        {
            CreateBranch = branchName =>
                repository.Value.CreateBranch(branchName,
                    target: defaultBranch.Value.Branch.Tip);

            Branches = branchLookup;
        }
    }
}
