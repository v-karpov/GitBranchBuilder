using CSharpFunctionalExtensions;
using GitBranchBuilder.Components.Holders;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// Реализация <see cref="IBuildProvider"/> на основе MSBuild
    /// </summary>
    public class BuildProvider : Provider<BuildEngineResult>, IBuildProvider
    {
        /// <summary>
        /// Возвращает результат построения проекта по указанному пути
        /// </summary>
        /// <param name="project">Проект для построения</param>
        /// <param name="logger">Логгер, при помощи которого будут выведены сообщения о результате построения</param>
        /// <returns></returns>
        public BuildEngineResult GetValue(Project project, Maybe<ILogger> logger) 
            => new BuildEngineResult(logger.HasValue 
                    ? project.Build(logger.Value)
                    : project.Build()
                , default);

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="project">Проект, который требуется построить</param>
        /// <param name="logger"></param>
        public BuildProvider(
            Holder<Project> project,
            MaybeHolder<ILogger> logger)
        {
            ValueGetter = () =>  GetValue(project, logger.Maybe);
        }
    }
}
