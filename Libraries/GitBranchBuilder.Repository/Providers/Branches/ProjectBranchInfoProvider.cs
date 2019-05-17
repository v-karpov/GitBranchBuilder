using System;
using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Components;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Провайдер информации о ветви на основе метки проекта, к которому она принадлежит
    /// </summary>
    public class ProjectBranchInfoProvider : IBranchInfoProvider
    {
        /// <summary>
        /// Метки проекта, которые считаются допустимыми при определении индетификатора
        /// </summary>
        protected HashSet<string> ProjectLabels { get; }

        /// <summary>
        /// Символ, при помощи которого производится дополнение идентфикатора
        /// </summary>
        protected char PadChar { get; }

        /// <summary>
        /// Ширина, до которой дополняется идентификатор
        /// </summary>
        protected int PadWidth { get; }

        /// <summary>
        /// Символы, использующиеся при разделении имени ветви на метку проекта и метку тикета
        /// </summary>
        protected char[] NameSplitters { get; }

        /// <summary>
        /// Возвращает информацию о ветви на основе ее имени
        /// </summary>
        /// <param name="branchName">Имя ветви в системе контроля версий</param>
        /// <returns></returns>
        public BranchInfo GetBranchInfo(string branchName)
            => GetBranchInfo(branchName, default);

        /// <summary>
        /// Возвращает информацию о ветви на основе ее имени
        /// </summary>
        /// <param name="branchName">Имя ветви в системе контроля версий</param>
        /// <param name="tag">Тег создаваемого объекта <see cref="BranchInfo"/></param>
        /// <returns></returns>
        public BranchInfo GetBranchInfo(string branchName, object tag)
        {
            var parts = branchName.Split(NameSplitters, StringSplitOptions.RemoveEmptyEntries);
            var label = parts
                .Select((x, index) => (value: x.ToLower(), index))
                .Where(x => ProjectLabels.Contains(x.value))
                .FirstOrDefault();

            var id = parts.Length > label.index + 1
                ? parts[label.index + 1]
                : default;

            return new BranchInfo(branchName, id?.PadLeft(PadWidth, PadChar), tag);
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="configurationProvider">Провайдер конфигурации системы</param>
        public ProjectBranchInfoProvider(ConfigurationHolder configuration)
        {
            ProjectLabels = new HashSet<string>(
                configuration.Array("Projects", "Labels")
                .Select(x => x.ToLower()));

            NameSplitters = configuration.CharArray("Projects", "Splitters");
            PadChar = configuration.Char("Projects", "PadChar");
            PadWidth = configuration.Int("Projects", "PadWidth");
        }
    }
}
