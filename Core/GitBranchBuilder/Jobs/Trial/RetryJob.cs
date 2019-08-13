using System;
using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Абстрактный класс, описывающий работу, которая может завершиться неуспешно, но может быть перезапущена
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    public abstract class RetryJob<TInput> : RetryJobBase<ITrialJob<TInput, Result>, TInput, Result>
    {

    }

    /// <summary>
    /// Абстрактный класс, описывающий работу, которая может завершиться неуспешно, но может быть перезапущена
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    /// <typeparam name="TResult">Тип результаа</typeparam>
    public abstract class RetryJob<TInput, TResult> : RetryJobBase<ITrialJob<TInput, Result<TResult>>, TInput, Result<TResult>>
    {
        protected override void FailureNotify(Result<TResult> result)
            => Log.Warn($"Unable to proceed step {WrappedJob} because of {result.Error}");
    }
}
