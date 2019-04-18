using SharpConfig;

namespace GitBranchBuilder.Core
{
    /// <summary>
    /// Провайдер конфигурации по умолчанию
    /// </summary>
    public class DefaultConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        /// Путь файла настроек по умолчанию
        /// </summary>
        private const string SettingsFilePath = "settings.cfg";

        /// <summary>
        /// Объект, представляющий настройки
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Перезагружает настройки из связанного файла
        /// </summary>
        public void Reload() => Configuration = Configuration.LoadFromFile(SettingsFilePath);

        /// <summary>
        /// Сохраняет настройки в связанный файл
        /// </summary>
        public void Save() => Configuration.SaveToFile(SettingsFilePath);

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DefaultConfigurationProvider() => Reload();
    }
}
