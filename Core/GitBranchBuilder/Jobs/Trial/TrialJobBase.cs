using System;

using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Базовый класс для работы, предоставляющей результат выполнения
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    /// <typeparam name="TResult">Тип результата выполнения</typeparam>
    public abstract class TrialJobBase<TInput, TResult> : PropagationJob<TInput, TResult>, ITrialJob<TInput, TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Функция, предоставляющая формат исключения
        /// </summary>
        /// <param name="exception">Исключение, сообщение которого необходимо форматировать</param>
        /// <returns></returns>
        protected string FormatException(Exception exception)
            => $"Unhandled exception:  {exception.GetType()} \r\n {exception.Message} \r\n {exception.StackTrace}";
    }
}
