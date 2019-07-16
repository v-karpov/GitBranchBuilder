using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Providers;
using LibGit2Sharp;
using System.Collections.Generic;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Компонент, содержащий в себе ссылку на сервер удаленного репозитория
    /// </summary>
    public class RemoteHolder : Holder<Remote>
    {
        /// <summary>
        /// Отображаемое название сервера
        /// </summary>
        public string Name => Value.Name;

        /// <summary>
        /// Набор спецификаций сервера
        /// </summary>
        public IEnumerable<RefSpec> FetchRefSpecs => Value.FetchRefSpecs;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="provider">Провайдер типа <see cref="Remote"/></param>
        public RemoteHolder(IProvider<Remote> provider)
            : base(provider)
        {
            
        }
    }
}
