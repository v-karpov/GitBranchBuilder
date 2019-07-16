using GitBranchBuilder.Components.Holders.Specific;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// Провайдер опций сборки проекта
    /// </summary>
    public class ProjectOptionsProvider : Provider<ProjectOptions>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="config">Конфигурация конвейера</param>
        public ProjectOptionsProvider(ConfigurationHolder config)
        {
            ValueGetter = () => new ProjectOptions
            {
                LoadSettings = (ProjectLoadSettings)config.Int("Build", "Settings")
            };
        }
    }
}
