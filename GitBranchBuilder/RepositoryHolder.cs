using LibGit2Sharp;

namespace GitBranchBuilder
{
    public abstract class RepositoryHolder
    {
        protected virtual Repository Repository { get; }

        public RepositoryHolder(IRepositoryProvider repositoryProvider)
        {
            Repository = repositoryProvider.Repository;
        }
    }
}
