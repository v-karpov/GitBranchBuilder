using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using GitBranchBuilder.Providers;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Хранилище множества значений из различных реализаций провайдера
    /// </summary>
    /// <typeparam name="T">Тип получаемых значений</typeparam>
    public class MultiHolder<T> : Lazy<IEnumerable<T>>, IEnumerable<T>, IComponent
    {
        /// <summary>
        /// Получает перечислитель для коллекции значений
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();

        /// <summary>
        /// Явная реализация <see cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Конструктор по умолчанию для инъекции множества провайдеров
        /// </summary>
        /// <param name="providers">Провайдеры, предоставляющие значения типа <see cref="T"/></param>
        public MultiHolder(IEnumerable<IProvider<T>> providers) :
            base(valueFactory: () => providers.Select(x => x.GetValue()),
                 mode: LazyThreadSafetyMode.PublicationOnly)
        {

        }

        /// <summary>
        /// Оператор неявного преобразования, получающий первое значение или значение по умолчанию
        /// </summary>
        /// <param name="holder">Хранилище значений</param>
        public static implicit operator T(MultiHolder<T> holder)
            => holder.FirstOrDefault();
    }
}
