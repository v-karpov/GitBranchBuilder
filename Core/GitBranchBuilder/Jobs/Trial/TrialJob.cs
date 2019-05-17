using System;

using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Интерфейс, описывающий работу, которая может быть завершена неуспешно
    /// </summary>
    /// <typeparam name="TInput">Тип выходных данных</typeparam>
    /// <typeparam name="TResult">Тип результата</typeparam>
    public interface ITrialJob<TInput, TResult> : IJob<TInput, TResult>
        where TResult : IResult
    {

    }

    /// <summary>
    /// Класс, описывающий работу, которая может быть завершена неуспешно
    /// </summary>
    /// <typeparam name="TInput">Тип выходных данных</typeparam>
    public abstract class TrialJob<TInput> : TrialJobBase<TInput, Result>
    {
        /// <summary>
        /// Действие, выполнение должно быть успешным для завершения задачи
        /// </summary>
        public Action TryProcess { get; protected set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public TrialJob() : base()
        {
            Process = () => Result.Try(TryProcess, FormatException);
        }
    }

    /// <summary>
    /// Класс, описывающий работу с результатом <see cref="Result{T}"/>
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class TrialJob<TInput, TOutput> : TrialJobBase<TInput, Result<TOutput>>
    {
        /// <summary>
        /// Функция, результат которой возвращается в случае успешного выполнения
        /// </summary>
        public Func<TOutput> TryProcess { get; protected set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public TrialJob() : base()
        {
            Process = () => Result.Try(TryProcess, FormatException);
        }
    }
}
