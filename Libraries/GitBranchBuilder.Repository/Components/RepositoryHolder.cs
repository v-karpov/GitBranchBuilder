using GitBranchBuilder.Providers;
using LibGit2Sharp;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Компонент, содержащий в себе ссылку на репозиторий проекта
    /// </summary>
    public class RepositoryHolder : Holder<Repository>
    {
        /// <summary>
        /// 
        /// </summary>
        public BranchCollection Branches => Value.Branches;

        /// <summary>
        /// 
        /// </summary>
        public Network Network => Value.Network;

        /// <summary>
        /// 
        /// </summary>
        public IQueryableCommitLog Commits => Value.Commits;

        /// <summary>
        /// 
        /// </summary>
        public Branch Head => Value.Head;

        public Index Index => Value.Index;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="repositoryProvider">Провайдер репозитория <see cref="LibGit2Sharp"/></param>
        public RepositoryHolder(IRepositoryProvider repositoryProvider)
            : base(repositoryProvider)
        {
            
        }
    }
}
