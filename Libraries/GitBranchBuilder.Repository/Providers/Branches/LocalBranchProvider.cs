using System;
using System.Linq;

using GitBranchBuilder.Components;
using GitBranchBuilder.Components.Holders;
using LibGit2Sharp;

using ILocalBranchLookup = System.Linq.ILookup<string, LibGit2Sharp.Branch>;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Интерфейс провайдера локальных веток репозитория
    /// </summary>
    public interface ILocalBranchProvider : IBranchProvider
    {

    }

    /// <summary>
    /// Реалзиация провайдера локальных веток репозитория
    /// </summary>
    public class LocalBranchProvider : ILocalBranchProvider
    {
        /// <summary>
        /// Реализация провайдера хранилища локальных веток
        /// </summary>
        private class LocalBranchLookupProvider : Provider<ILocalBranchLookup>
        {
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            /// <param name="repository">Репозиторий, для которого производится поиск</param>
            public LocalBranchLookupProvider(
                RepositoryHolder repository)
            {
                ValueGetter = () =>
                    repository.Branches
                        .Where(x => !x.IsRemote)
                        .ToLookup(x => x.FriendlyName);
            }
        }

        /// <summary>
        /// Хралищие локальных веток репозитория
        /// </summary>
        public Holder<ILocalBranchLookup> Branches { get; }

        /// <summary>
        /// Функция, позволяющая создать ветку по ее названию
        /// </summary>
        public Func<string, Branch> CreateBranch { get; }

        /// <summary>
        /// Получает или создает локальную ветку по ее названию
        /// </summary>
        /// <param name="name">Название локальной ветки</param>
        /// <returns></returns>
        protected Branch GetOrCreate(string name)
            => Branches.Value[name].SingleOrDefault() ?? CreateBranch(name);

        /// <summary>
        /// Возвращает ветку, соответствующую описанию
        /// </summary>
        /// <param name="info">Описание ветки для поиска</param>
        /// <returns>Существующая или созданная локальная ветка</returns>
        public Branch GetBranch(BranchInfo info)
            => GetOrCreate(info.Name);

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="repository">Репозиторий, для которого производится поиск</param>
        /// <param name="defaultBranch">Ветка по умолчанию</param>
        /// <param name="branchLookup">Хранилище локальных веток</param>
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
