using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Абстрактный класс, описывающий болк работы
    /// </summary>
    /// <typeparam name="TInput">Входной тип данных</typeparam>
    /// <typeparam name="TOutput">Тип результата</typeparam>
    public abstract class Job<TInput, TOutput> : IJob<TInput>, ISourceBlock<TOutput>
    {
        /// <summary>
        /// Описание работы, понятное для пользователя
        /// </summary>
        public virtual string Description => $"{GetType().Name}";

        /// <summary>
        /// Делегат для выполнения обработки задания
        /// </summary>
        protected Func<TOutput> Process { get; set; }

        /// <summary>
        /// Делегат для выполнения подготовительного этапа работы
        /// </summary>
        protected Action<TInput> Prepare { get; set; }

        /// <summary>
        /// Выполняет подготовительный и непосредственный этапы работы
        /// </summary>
        /// <param name="input">Входные данные для подготовительного этапа</param>
        /// <returns></returns>
        protected virtual TOutput Execute(TInput input)
        {
            Prepare?.Invoke(input);

            Console.WriteLine($"{Description}");

            return Process.Invoke();
        }

        /// <summary>
        /// Связывает данную работу с другой работой через интерфейс <see cref="ITargetBlock{TInput}"/>
        /// </summary>
        /// <param name="job">Работа, с которой требуется связать данный экзепляр</param>
        /// <returns>Экзепляр связанной работы</returns>
        /// <remarks>Позволяет применять связанывание во fluent-формате</remarks>
        public TJob LinkTo<TJob>(TJob job, Action<JobLink<TOutput>> handler = default)
            where TJob : IJob<TOutput>
        {
            var link = new JobLink<TOutput>(this, job);
            handler?.Invoke(link);

            return job;
        }

        /// <summary>
        /// Освобождает все ресурсы данного экземпляра работы
        /// </summary>
        public virtual void Dispose() { }

        #region ITargetBlock<PipelineStartOptions>

        /// <summary>
        /// Блок, получающий данные на вход и обрабатывающий их
        /// </summary>
        protected abstract ITargetBlock<TInput> TargetBlock { get; }

        /// <summary>
        /// Задача, сигнализирующая о завершении обработки данных блоком
        /// </summary>
        public virtual Task Completion => TargetBlock.Completion;

        DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
            => TargetBlock.OfferMessage(messageHeader, messageValue, source, consumeToAccept);

        void IDataflowBlock.Complete()
            => TargetBlock.Complete();

        void IDataflowBlock.Fault(Exception exception)
            => TargetBlock.Fault(exception);

        #endregion

        #region ISourceBlock<TResult>

        /// <summary>
        /// Блок, обрабатывающий данные и передающий результат
        /// </summary>
        protected abstract ISourceBlock<TOutput> SourceBlock { get; }

        IDisposable ISourceBlock<TOutput>.LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
            => SourceBlock.LinkTo(target, linkOptions);

        TOutput ISourceBlock<TOutput>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
            => SourceBlock.ConsumeMessage(messageHeader, target, out messageConsumed);

        bool ISourceBlock<TOutput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
            => SourceBlock.ReserveMessage(messageHeader, target);

        void ISourceBlock<TOutput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
            => SourceBlock.ReleaseReservation(messageHeader, target);

        #endregion
    }
}
