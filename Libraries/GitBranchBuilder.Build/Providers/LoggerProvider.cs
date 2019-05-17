using System;

using GitBranchBuilder.Components;

using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// Провайдер логгера для <see cref="IBuildProvider"/>
    /// </summary>
    public class LoggerProvider : Provider<ILogger>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="config">Конфигурация конвейера</param>
        public LoggerProvider(ConfigurationHolder config)
        {
            ValueGetter = () => 
                Enum.TryParse(config["Build", "Verbosity"], out LoggerVerbosity verbosity) 
                    ? new ConsoleLogger(verbosity) 
                    : default; 
        }
    }
}
