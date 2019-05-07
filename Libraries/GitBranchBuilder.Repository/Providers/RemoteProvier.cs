using GitBranchBuilder.Components;
using LibGit2Sharp;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Провайдер ссылки на удаленный сервер
    /// </summary>
    public interface IRemoteProvider : IProvider<Remote>
    {

    }

    public class OriginRemoteProvdier : Provider<Remote>, IRemoteProvider
    {
        /// <summary>
        /// Название удаленного сервера для поиска в списке
        /// </summary>
        public const string RemoteName = "origin";

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="repository">Репозиторий <see cref="LibGit2Sharp"/></param>
        public OriginRemoteProvdier(RepositoryHolder repository) 
            => ValueGetter = () => repository.Network.Remotes[RemoteName];
    }
}
