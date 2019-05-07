using GitBranchBuilder.Providers;
using SharpConfig;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Базовый компонент, содержащий конфигурацию
    /// </summary>
    public class ConfigurationHolder : Holder<Configuration>
    {
        #region [Методы]

        /// <summary>
        /// Получает данные конкретного ключа конфигурации
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации</returns>
        protected Setting GetSetting(string section, string key)
            => Value[section][key];

        /// <summary>
        /// Получает данные конкретного ключа конфигурации в виде массива строк
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации в виде массива строк</returns>
        public string[] Array(string section, string key)
            => GetSetting(section, key).StringValueArray;

        /// <summary>
        /// Получает данные конкретного ключа конфигурации в виде единичного символа
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации в виде единичного символа</returns>
        public char Char(string section, string key)
            => GetSetting(section, key).CharValue;

        /// <summary>
        /// Получает данные конкретного ключа конфигурации в виде массива символов
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации в видле массива символов</returns>
        public char[] CharArray(string section, string key)
            => GetSetting(section, key).CharValueArray;

        /// <summary>
        /// Получает данные конкретного ключа конфигурации в виде целого числа
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации в виде целого числа</returns>
        public int Int(string section, string key)
            => GetSetting(section, key).IntValue;

        /// <summary>
        /// Получает данные конкретного ключа конфигурации в виде массива целых чисел
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации</returns>
        public int[] IntArray(string section, string key)
            => GetSetting(section, key).IntValueArray;

        #endregion

        /// <summary>
        /// Возвращает секцию с указанным именем
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <returns></returns>
        public Section this[string section] => Value[section];

        /// <summary>
        /// Индексатор, предоставляющий настройку в виде текстового значения
        /// </summary>
        /// <param name="section">Имя секции конфигурации</param>
        /// <param name="key">Имя ключа конфигурации</param>
        /// <returns>Данные ключа конфигурации в виде строки</returns>
        public string this[string section, string key]
        {
            get => GetSetting(section, key).StringValue;
            set => GetSetting(section, key).StringValue = value;
        }

        /// <summary>
        /// Конструктор компонента по умолчанию
        /// </summary>
        /// <param name="configurationProvider"></param>
        public ConfigurationHolder(IConfigurationProvider configurationProvider)
            : base(configurationProvider)
        {

        }
    }
}
