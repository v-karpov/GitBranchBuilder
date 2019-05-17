using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Нетипизированная работа, являющаяся концом конвейера
    /// </summary>
    public interface IFinalJob : ISourceBlock<PipelineResult>
    {
        Task<PipelineResult> Result { get; }
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
        Job<TInput, PipelineResult>, IFinalJob<TInput>
    {
        /// <summary>
        /// Результат выполнения данной ветви конвейера
        /// </summary>
        public Task<PipelineResult> Result => CompletionSource.Task;

        public TaskCompletionSource<PipelineResult> CompletionSource { get; }

        /// <summary>
        /// Блок, устанавливающий результат
        /// </summary>
        protected virtual ActionBlock<TInput> ResultSetter { get; }

        protected override ITargetBlock<TInput> TargetBlock => ResultSetter;

        protected override ISourceBlock<PipelineResult> SourceBlock =>
            throw new InvalidOperationException();

        public FinalJob() : base()
        {
            CompletionSource = new TaskCompletionSource<PipelineResult>();
            ResultSetter = new ActionBlock<TInput>(x => CompletionSource.SetResult(Execute(x)));
        }
    }
}
