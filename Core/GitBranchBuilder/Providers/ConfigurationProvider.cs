using SharpConfig;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Провайдер конфигурации для системы
    /// </summary>
    public interface IConfigurationProvider : IProvider<Configuration>
    {
        /// <summary>
        /// Сохраняет конфигурацию в исходный объект
        /// </summary>
        void Save(Configuration configuration);
    }

    /// <summary>
    /// Провайдер конфигурации для системы по умолчанию
    /// </summary>
    public class DefaultConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        /// Путь файла настроек по умолчанию
        /// </summary>
        private const string SettingsFilePath = "settings.cfg";

        /// <summary>
        /// Сохраняет настройки в связанный файл
        /// </summary>
        public void Save(Configuration configuration)
            => configuration.SaveToFile(SettingsFilePath);

        /// <summary>
        /// Возвращает загруженное значение настроек из файла <see cref="SettingsFilePath"/>
        /// </summary>
        /// <returns></returns>
        public Configuration GetValue()
            => Configuration.LoadFromFile(SettingsFilePath);
    }
}
