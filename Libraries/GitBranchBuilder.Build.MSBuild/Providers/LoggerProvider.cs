using System;

using GitBranchBuilder.Components;

using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace GitBranchBuilder.Providers.Build
{
    public class LoggerProvider : Provider<ILogger>
    {
        public LoggerProvider(ConfigurationHolder config)
        {
            ValueGetter = () => new ConsoleLogger(
                Enum.TryParse(config["Build", "Verbosity"], out LoggerVerbosity verbosity)
                ? verbosity
                : LoggerVerbosity.Normal);
        }
    }
}
