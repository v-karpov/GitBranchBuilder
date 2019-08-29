using System;
using System.Collections.Generic;
using System.Linq;

using CSharpFunctionalExtensions;

using GitBranchBuilder.Components;
using GitBranchBuilder.Components.Holders;

using LibGit2Sharp;

namespace GitBranchBuilder.Jobs
{
    public class FetchJob<TInput> : TrialJob<TInput>
    {
        /// <summary>
        /// Описание работы для вывода на экран
        /// </summary>
        public override string Description => $"Fetching from {Remote.Name}...";

        /// <summary>
        /// Спецификации ссылок (<see cref="RefSpec"/>)
        /// </summary>
        public IEnumerable<RefSpec> RefSpecs => Remote.FetchRefSpecs;

        /// <summary>
        /// Ссылка на репозиторий на сервере
        /// </summary>
        public RemoteHolder Remote { get; set; }

        /// <summary>
        /// Репозиторий, для которого производится получение удаленного состояния
        /// </summary>
        public RepositoryHolder Repository { get; set; }

        /// <summary>
        /// Опции получения состояния удаленного репозитория
        /// </summary>
        public Holder<FetchOptions> FetchOptions { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="prepareAction">Подготовительное действие</param>
        /// <param name="logMessage">Сообщение, которое будет записано в лог по окончании получения состояния</param>
        public FetchJob(Action<TInput> prepareAction = default, string logMessage = default)
        {
            Prepare = prepareAction ?? Prepare;

            TryProcess = () => Commands.Fetch(
                Repository, 
                Remote.Name,
                RefSpecs.Select(spec => spec.Specification),
                FetchOptions,
                logMessage ?? string.Empty);
        }
    }
}
