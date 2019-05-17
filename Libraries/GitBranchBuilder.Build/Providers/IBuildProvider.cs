using CSharpFunctionalExtensions;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// Интерфейс провайдера результата построения проекта
    /// </summary>
    public interface IBuildProvider : IProvider<BuildEngineResult>
    {
        /// <summary>
        /// Возвращает результат построения проекта по указанному пути
        /// </summary>
        /// <param name="project">Проект для построения</param>
        /// <param name="logger">Логгер, при помощи которого будут выведены сообщения о результате построения</param>
        /// <returns></returns>
        BuildEngineResult GetValue(Project project, Maybe<ILogger> logger);
    }
}
