using GitBranchBuilder.Providers;

using NLog;
using NLog.Config;
using NLog.Targets;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Сервис, позволяющий получить подтверждение пользователя через консоль
    /// </summary>
    public class ConsoleLoggerProvider : Provider<ILogger>
    {
        public LoggingConfiguration Configuration { get; } 
            = new LoggingConfiguration();

        public ConsoleLoggerProvider()
        {
            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = "${date:format=HH\\:mm\\:ss.fff} ${message} ${exception}",
                UseDefaultRowHighlightingRules = true,
            };

            Configuration.AddTarget("console", consoleTarget);
            Configuration.AddRuleForAllLevels(consoleTarget);

            LogManager.Configuration = Configuration;

            ValueGetter = () => LogManager.GetLogger("console");
        }
    }
}