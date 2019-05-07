using System;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Провайдер по умолчанию для всех типов, имеющих беспараметрический конструктор
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DefaultProvider<T> : IProvider<T>
    {
        static readonly object[] EmptyParams = new object[0];

        /// <summary>
        /// Функция получения значения провайдера, реализованная
        /// через ограничение new() для типа <typeparamref name="T"/>
        /// </summary>
        /// <returns></returns>
        public T GetValue()
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), EmptyParams);
            }
            catch
            {
                return default;
            }
        }
    }
}
