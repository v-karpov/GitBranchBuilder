using System;
using System.Collections.Generic;
using System.Linq;

using SharpConfig;

namespace GitBranchBuilder
{
    /// <summary>
    /// Провайдер информации о ветви на основе метки проекта, к которому она принадлежит
    /// </summary>
    public class ProjectBranchInfoProvider : ConfigurationHolder, IBranchInfoProvider
    {
        /// <summary>
        /// Символ, используемый по умолчанию для дополнения идентификатора до длины <see cref="DefaultPadWidth"/>
        /// </summary>
        public const char DefaultPadChar = '0';

        /// <summary>
        /// Длина, до которой по умолчанию дополняется слева идентификатор при помощи символов <see cref="DefaultPadChar"/>
        /// </summary>
        public const int DefaultPadWidth = 3;

        /// <summary>
        /// Секция настроек проекта
        /// </summary>
        protected Section ProjectsSettings { get; }

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
        /// Разделители, используемые для обработки имени ветки по умолчанию
        /// </summary>
        static readonly char[] DefaultBranchSplitters = new[] { '-', '_' };

        /// <summary>
        /// Возвращает информацию о ветви на основе ее имени
        /// </summary>
        /// <param name="branchName">Имя ветви в системе контроля версий</param>
        /// <returns></returns>
        public BranchInfo GetBranchInfo(string branchName)
        {
            var parts = branchName.Split(NameSplitters, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1 && ProjectLabels.Contains(parts[0].ToLower()))
            {
                var id = parts[1].PadLeft(PadWidth, PadChar);

                return new BranchInfo(branchName, id);
            }

            return new BranchInfo(branchName);
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="configurationProvider">Провайдер конфигурации системы</param>
        public ProjectBranchInfoProvider(IConfigurationProvider configurationProvider)
            : base(configurationProvider)
        {
            if (Configuration?.Contains("Projects") ?? false)
            {
                ProjectsSettings = Configuration["Projects"];

                ProjectLabels = ProjectsSettings.Contains("Labels")
                    ? ProjectsSettings["Labels"].StringValueArray
                        .Select(x => x.ToLower())
                        .ToHashSet()
                    : new HashSet<string>();

                NameSplitters = ProjectsSettings.Contains("Splitters")
                    ? ProjectsSettings["Splitters"].StringValueArray
                        .Select(x => x[0])
                        .ToArray()
                    : DefaultBranchSplitters;

                PadChar = ProjectsSettings.Contains("PadChar")
                    ? ProjectsSettings["PadChar"].CharValue
                    : DefaultPadChar;

                PadWidth = ProjectsSettings.Contains("PadWidth")
                    ? ProjectsSettings["PadWidth"].IntValue
                    : DefaultPadWidth;
            }
        }
    }
}
