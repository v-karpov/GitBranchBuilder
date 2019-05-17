using System.Linq;
using GitBranchBuilder.Components;
using LibGit2Sharp;

using IRemoteBranchLookup = System.ValueTuple<LibGit2Sharp.Remote, System.Linq.ILookup<string, LibGit2Sharp.Branch>>;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Интерфейс провайдера удаленных веток, привязанных к определенному <see cref="Remote"/>
    /// </summary>
    public interface IRemoteBranchProvider : IBranchProvider
    {
        /// <summary>
        /// Ссылка на сервер репозитория
        /// </summary>
        Remote Remote { get; }
    }

    /// <summary>
    /// Реализация провайдера удаленных ссылок
    /// </summary>
    public class RemoteBranchProvider : IRemoteBranchProvider
    {
        /// <summary>
        /// Реализация провайдера хранилища ссылок
        /// </summary>
        private class RemoteBranchLookupProvider : Provider<IRemoteBranchLookup>
        {
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            /// <param name="repository">Репозиторий, для которого производится поиск</param>
            /// <param name="remote">Ссылка на сервер репозитория</param>
            public RemoteBranchLookupProvider(
                RepositoryHolder repository,
                RemoteHolder remote)
            {
                ValueGetter = () =>
                {
                    var branchLookup = repository.Branches
                        .Where(x => x.IsRemote)
                        .ToLookup(x => x.FriendlyName
                            .Replace(remote.Name + "/", ""));

                    return (remote, branchLookup);
                };
            }
        }

        /// <summary>
        /// Ссылка на сервер репозитория
        /// </summary>
        public Remote Remote => Branches.Value.Item1;

        /// <summary>
        /// Хранилище ссылок на ветки
        /// </summary>
        public Holder<IRemoteBranchLookup> Branches { get; set; }

        /// <summary>
        /// ВОзвращает удаленную ветку по ее описанию
        /// </summary>
        /// <param name="info">Описание удаленной ветки</param>
        /// <returns></returns>
        public Branch GetBranch(BranchInfo info)
            => Branches.Value.Item2[info.Name]
                .SingleOrDefault();
    }
}
