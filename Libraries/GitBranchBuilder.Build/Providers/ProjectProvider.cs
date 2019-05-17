using GitBranchBuilder.Components;

using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// Провайдер проекта <see cref="Project"/>
    /// </summary>
    public class ProjectProvider : Provider<Project>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="buildPath">Путь к проекту</param>
        /// <param name="options">Опции построения проекта</param>
        public ProjectProvider(
            Holder<BuildPath> buildPath,
            Holder<ProjectOptions> options)
        {
            ValueGetter = () => Project.FromFile(buildPath.Value.FullPath, options);
        }
    }
}
