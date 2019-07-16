using GitBranchBuilder.Components.Holders;
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
        /// Коллекция веток репозитория
        /// </summary>
        public BranchCollection Branches => Value.Branches;

        /// <summary>
        /// Сетевые параметры репозитория
        /// </summary>
        public Network Network => Value.Network;

        /// <summary>
        /// Обозреватель лога коммитов
        /// </summary>
        public IQueryableCommitLog Commits => Value.Commits;

        /// <summary>
        /// "Голова" выбранной ветки репозитория
        /// </summary>
        public Branch Head => Value.Head;

        /// <summary>
        /// Индекс репозитория
        /// </summary>
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
