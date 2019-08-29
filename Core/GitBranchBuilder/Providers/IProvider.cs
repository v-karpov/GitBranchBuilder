using System;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Интерфейс провайдера данных\объектов системы
    /// </summary>
    public interface IProvider
    {

    }

    /// <summary>
    /// Интерфейс провайдера данных\объектов системы
    /// </summary>
    public interface IProvider<out T> : IProvider
    {
        /// <summary>
        /// Получает связанный объект
        /// </summary>
        /// <returns></returns>
        T GetValue();
    };
}
