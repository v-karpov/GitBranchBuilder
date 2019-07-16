using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Наболр клавиш, содержащий только указанные клавиши
    /// </summary>
    public class SpecificKeySet : IKeySet
    {
        /// <summary>
        /// Набор клавиш по умолчанию, описывающий дейсвтие подтверждения
        /// </summary>
        public static IKeySet ApproveDefault { get; } = new SpecificKeySet(ConsoleKey.Enter, ConsoleKey.Y, ConsoleKey.Spacebar);

        /// <summary>
        /// Набор клавиш по умолчанию, описывающий действие отказа
        /// </summary>
        public static IKeySet RefuseDefault { get; } = new SpecificKeySet(ConsoleKey.Escape, ConsoleKey.N);

        /// <summary>
        /// Набор клавиш
        /// </summary>
        public HashSet<ConsoleKey> Keys { get; }
        /// <summary>
        /// Описание набора клавиш
        /// </summary>
        public string Description =>
            Keys.Count > 1
            ? $"one of ({string.Join(", ", Keys.Select(GetKeyName))})"
            : GetKeyName(Keys.First()) ?? "(none)";

        /// <summary>
        /// Возвращает название конкретной клавиши 
        /// </summary>
        /// <param name="key">Клавиша, название которой необходимо получить</param>
        /// <returns></returns>
        private string GetKeyName(ConsoleKey key) => Enum.GetName(typeof(ConsoleKey), key);

        /// <summary>
        /// Проверяет, содержит ли набор указанную клавишу
        /// </summary>
        /// <param name="key">Клавиша для проверки</param>
        /// <returns></returns>
        public bool Contains(ConsoleKey key) => Keys.Contains(key);

        /// <summary>
        /// Возвращает объект <see cref="IEnumerator{ConsoleKey}"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ConsoleKey> GetEnumerator() => Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public SpecificKeySet(params ConsoleKey[] keys)
        {
            Keys = new HashSet<ConsoleKey>(keys);
        }
    }
}