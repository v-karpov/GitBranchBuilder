using System;
using System.Collections.Generic;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Интерфейс набор клавиш
    /// </summary>
    public interface IKeySet : IEnumerable<ConsoleKey>, IComponent
    {
        /// <summary>
        /// Описание набора клавиш
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Проверяет, содержит ли набор указанную клавишу
        /// </summary>
        /// <param name="key">Клавиша для проверки</param>
        /// <returns></returns>
        bool Contains(ConsoleKey key);
    }
}