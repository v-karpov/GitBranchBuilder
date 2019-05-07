using GitBranchBuilder.Components;
using LibGit2Sharp;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Интерфейс, описывающий провайдер репозитория <see cref="LibGit2Sharp"/>
    /// </summary>
    public interface IRepositoryProvider : IProvider<Repository>
    {

    }

    /// <summary>
    /// Провайдер репозитория <see cref="LibGit2Sharp"/> по умолчанию
    /// </summary>
    public class DefaultRepositoryProvider : Provider<Repository>, IRepositoryProvider
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="options">Настройки репозитория</param>
        /// <param name="configuration">Конфигурация</param>
        public DefaultRepositoryProvider(
            Holder<RepositoryOptions> options,
            ConfigurationHolder configuration)
        {
            ValueGetter = () =>
                new Repository(configuration["Repository", "Path"], options);
        }
    }
}
