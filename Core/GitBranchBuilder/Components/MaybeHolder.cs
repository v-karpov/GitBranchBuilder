using CSharpFunctionalExtensions;

using GitBranchBuilder.Providers;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Хранилище опциональных данных (использует тип <see cref="Maybe{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaybeHolder<T> : Holder<Maybe<T>>
    {
        /// <summary>
        /// Провайдер значения <see cref="Maybe{T}"/> из <see cref="T"/>
        /// </summary>
        public class MaybeProvider : Provider<Maybe<T>>
        {
            /// <summary>
            /// Конструктор, предоставляющий преобразование
            /// </summary>
            /// <param name="provider">Провайдер типа <see cref="T"/></param>
            public MaybeProvider(IProvider<T> provider)
            {
                ValueGetter = () => Maybe<T>.From(provider.GetValue());
            }
        }

        /// <summary>
        /// Опциональное значение
        /// </summary>
        public Maybe<T> Maybe => base.Value;

        /// <summary>
        /// Определено ли значение
        /// </summary>
        public bool HasValue => Maybe.HasValue;

        /// <summary>
        /// Не определено ли значение
        /// </summary>
        public bool HasNoValue => Maybe.HasNoValue;

        /// <summary>
        /// Определенное знчаение или ошибка в случае его отсутствия
        /// </summary>
        public new T Value => Maybe.Value;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="provider"></param>
        public MaybeHolder(IProvider<T> provider) :
            this(new MaybeProvider(provider))
        {
           
        }

        /// <summary>
        /// Внутренний конструктор, позволяющий использовать сервис с поддержкой <see cref="Maybe{T}"/> напрямую
        /// </summary>
        /// <param name="provider"></param>
        internal MaybeHolder(IProvider<Maybe<T>> provider) :
            base(provider)
        {

        }

        /// <summary>
        /// Оператор неявного преобразования в тип <see cref="Maybe{T}"/>
        /// </summary>
        /// <param name="multiHolder"></param>
        public static implicit operator Maybe<T>(MaybeHolder<T> multiHolder)
            => multiHolder.Maybe;

        /// <summary>
        /// Оператор неявного преобразования в тип <see cref="T"/>
        /// </summary>
        /// <param name="multiHolder"></param>
        /// <exception cref="System.InvalidOperationException">Выбрасывается в случае отсутствия значения</exception>
        public static implicit operator T(MaybeHolder<T> multiHolder)
            => multiHolder.Value;
    }
}
