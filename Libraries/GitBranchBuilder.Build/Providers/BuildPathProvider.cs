using System.IO;

using GitBranchBuilder.Components;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// Провайдер пути проекта сборки
    /// </summary>
    public class BuildPathProvider : Provider<BuildPath>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="config">Конфигурация конвейера</param>
        public BuildPathProvider(ConfigurationHolder config)
        {
            ValueGetter = () => new BuildPath
            {
                FullPath = Path.Combine(config["Repository", "Path"], config["Build", "Path"])
            };
        }
    }
}
