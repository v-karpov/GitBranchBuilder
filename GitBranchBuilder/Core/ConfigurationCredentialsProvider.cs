using System;
using LibGit2Sharp;

using Section = SharpConfig.Section;

namespace GitBranchBuilder.Core
{
    /// <summary>
    /// Провайдер удостоверения на основе набора настроек <see cref="SharpConfig.Setting"/>
    /// </summary>
    public class ConfigurationCredentialsProvider : ConfigurationHolder, ICredentialsProvider
    {
        /// <summary>
        /// Секция в настройках, отвечающая за удостоверение
        /// </summary>
        protected Section CredentialsSections { get; }

        /// <summary>
        /// Удостоверение с отложенной загрузкой
        /// </summary>
        protected Lazy<Credentials> Credentials { get; }

        /// <summary>
        /// Получает загруженное или загружает новове удостоверение
        /// </summary>
        /// <param name="url">URL цели проверки удостоверения (не используется)</param>
        /// <param name="usernameFromUrl">Имя пользователя в URL (не используетя)</param>
        /// <param name="types">Поддерживаемые типы удостоверений (не используется)</param>
        /// <returns></returns>
        public Credentials GetCredentials(string url, string usernameFromUrl, SupportedCredentialTypes types)
            => Credentials.Value;

        /// <summary>
        /// Загруает удостоверение на основе полученных ранее настроек
        /// </summary>
        /// <returns></returns>
        protected virtual Credentials LoadCredentials()
            => new UsernamePasswordCredentials
            {
                Username = CredentialsSections["Login"].StringValue,
                Password = CredentialsSections["Password"].StringValue,
            };

        /// <summary>
        /// Конструктор по умолчанию для класса <see cref="ConfigurationCredentialsProvider"/>
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigurationCredentialsProvider(IConfigurationProvider configurationProvider)
            : base(configurationProvider)
        {
            Credentials = new Lazy<Credentials>(LoadCredentials);
            CredentialsSections = Configuration["Credentials"];
        }
    }
}
