using System;
using System.Threading;

using GitBranchBuilder.Providers;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Класс, описывающий держатель объекта <typeparamref name="T"/> на основе провайдера <see cref="IProvider{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Holder<T> : Lazy<T>, IComponent
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="provider">Провайдер объекта типа <see cref="T"/></param>
        public Holder(IProvider<T> provider)
            : base(valueFactory: provider.GetValue,
                   mode: LazyThreadSafetyMode.PublicationOnly)
        {

        }

        /// <summary>
        /// Оператор неявного преобразования значения <see cref="Holder{T}"/> в <typeparamref name="T"/>
        /// </summary>
        /// <param name="holder"></param>
        public static implicit operator T(Holder<T> holder) => holder.Value;
    }
}
