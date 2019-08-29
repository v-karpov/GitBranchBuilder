using System;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Базовая реализация интерфейса <see cref="IProvider{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Provider<T> : IProvider<T>
    {
        /// <summary>
        /// Делегат для получения значения
        /// </summary>
        /// <remarks>Рекомендуется использовать в связке с замыканиями</remarks>
        protected Func<T> ValueGetter { get; set; }

        /// <summary>
        /// Функция получения значения провайдера, реализованная через делегат <see cref="ValueGetter"/>
        /// </summary>
        /// <returns></returns>
        public virtual T GetValue() => ValueGetter();
    }
}
