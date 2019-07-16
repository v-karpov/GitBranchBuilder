using System.Collections.Generic;
using System.Collections.ObjectModel;

using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines.Configarable
{
    /// <summary>
    /// Абстрактный конфигуратор конвейера работ
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    public abstract class PipelineConfigurator<TJob> : IPipelineConfigurator<TJob>
        where TJob : IJob
    {
        /// <summary>
        /// Список всех работ конвейера
        /// </summary>
        public IReadOnlyCollection<TJob> JobCollection { get; protected set; }

        /// <summary>
        /// Начальная работа
        /// </summary>
        public IStartJob StartJob { get; protected set; }

        /// <summary>
        /// Функция конфигурации конвейера, возвращающая задачу,
        /// содержающую результат его выполнения
        /// </summary>
        public ResultConfigurator<TJob> ConfigureResult { get; protected set; }

        #region [Методы]

        /// <summary>
        /// Создает коллекцию, доступную только для чтения, из массива объектов
        /// </summary>
        /// <typeparam name="T">Тип объектов коллекции</typeparam>
        /// <param name="values">Массив объектов, из которых необходимо создать коллекцию</param>
        /// <returns></returns>
        protected IReadOnlyCollection<T> FromCollection<T>(params T[] values)
            => new ReadOnlyCollection<T>(values);

        /// <summary>
        /// Создает коллекцию работ, доступную только для чтения, из указанного массива работ
        /// </summary>
        /// <param name="jobs">Массив работ, из которых необходимо создать коллекцию</param>
        /// <returns></returns>
        protected IReadOnlyCollection<TJob> FromJobs(params TJob[] jobs)
            => FromCollection(jobs);

        /// <summary>
        /// Создает коллекцию с единственным элементом
        /// </summary>
        /// <typeparam name="T">Тип объектов коллекции</typeparam>
        /// <param name="element">Элемент, который нужно представить в виде коллекции</param>
        /// <returns></returns>
        protected IReadOnlyCollection<T> FromSingle<T>(T element)
            => FromCollection(element);

        #endregion
    }
}
