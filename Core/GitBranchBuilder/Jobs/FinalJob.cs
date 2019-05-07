using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Нетипизированная работа, являющаяся концом конвейера
    /// </summary>
    public interface IFinalJob : ISourceBlock<Result>
    {
        Task<Result> Result { get; }
    }


    /// <summary>
    /// Работа, являющаяся концом конвейера
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    public interface IFinalJob<TInput> : IJob<TInput>, IFinalJob
    {

    }

    /// <summary>
    /// Абстрактный класс, описывающий блок работы, являющийся финальным в своей ветви конвейера
    /// </summary>
    /// <typeparam name="TInput">Входной тип данных</typeparam>
    public abstract class FinalJob<TInput> :
        Job<TInput, Result>, IFinalJob<TInput>
    {
        /// <summary>
        /// Результат выполнения данной ветви конвейера
        /// </summary>
        public Task<Result> Result => CompletionSource.Task;

        public TaskCompletionSource<Result> CompletionSource { get; }

        /// <summary>
        /// Блок, устанавливающий результат
        /// </summary>
        protected virtual ActionBlock<TInput> ResultSetter { get; }

        protected override ITargetBlock<TInput> TargetBlock => ResultSetter;

        protected override ISourceBlock<Result> SourceBlock =>
            throw new InvalidOperationException();

        public FinalJob() : base()
        {
            CompletionSource = new TaskCompletionSource<Result>();
            ResultSetter = new ActionBlock<TInput>(x => CompletionSource.SetResult(Execute(x)));
        }
    }
}
