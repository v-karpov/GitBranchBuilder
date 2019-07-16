using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Набор клавиш, содержащий все доступные клавиши
    /// </summary>
    public class FullKeySet : IKeySet
    {
        /// <summary>
        /// Экземляр <see cref="FullKeySet"/>
        /// </summary>
        public static IKeySet Instance { get; } = new FullKeySet();

        /// <summary>
        /// Описание набора клавиш
        /// </summary>
        public string Description => "any key";

        /// <summary>
        /// Проверяет, содержит ли набор указанную клавишу
        /// </summary>
        /// <param name="key">Клавиша для проверки</param>
        /// <returns></returns>
        public bool Contains(ConsoleKey key) => true;

        /// <summary>
        /// Возвращает объект <see cref="IEnumerator{ConsoleKey}"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ConsoleKey> GetEnumerator()
            => Enum.GetValues(typeof(ConsoleKey)).Cast<ConsoleKey>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}