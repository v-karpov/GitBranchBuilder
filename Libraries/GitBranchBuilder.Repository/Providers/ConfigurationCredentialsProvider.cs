using GitBranchBuilder.Components;
using LibGit2Sharp;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Провайдер удостоверения
    /// </summary>
    public interface ICredentialsProvider : IProvider<Credentials>
    {
        /// <summary>
        /// Получает загруженное или загружает новове удостоверение
        /// </summary>
        /// <param name="url">URL цели проверки удостоверения (не используется)</param>
        /// <param name="usernameFromUrl">Имя пользователя в URL (не используетя)</param>
        /// <param name="types">Поддерживаемые типы удостоверений (не используется)</param>
        /// <returns></returns>
        Credentials GetValue(string url, string usernameFromUrl, SupportedCredentialTypes types);
    }

    /// <summary>
    /// Провайдер удостоверения на основе набора настроек <see cref="SharpConfig.Setting"/>
    /// </summary>
    public class ConfigurationCredentialsProvider : ICredentialsProvider
    {
        /// <summary>
        /// Конфигурация системы
        /// </summary>
        public ConfigurationHolder Configuration { get; set; }

        /// <summary>
        /// Загруает удостоверение на основе полученных ранее настроек
        /// </summary>
        /// <returns></returns>
        public Credentials GetValue()
            => new UsernamePasswordCredentials
            {
                Username = Configuration["Credentials", "Login"],
                Password = Configuration["Credentials", "Password"],
            };

        /// <summary>
        /// Получает загруженное или загружает новове удостоверение
        /// </summary>
        /// <param name="url">URL цели проверки удостоверения (не используется)</param>
        /// <param name="usernameFromUrl">Имя пользователя в URL (не используетя)</param>
        /// <param name="types">Поддерживаемые типы удостоверений (не используется)</param>
        /// <returns></returns>
        public Credentials GetValue(string url, string usernameFromUrl, SupportedCredentialTypes types)
            => GetValue();
    }
}
