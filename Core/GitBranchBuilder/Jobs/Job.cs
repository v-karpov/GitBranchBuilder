﻿using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GitBranchBuilder.Components.Holders;
using NLog;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Абстрактный класс, описывающий блок работы
    /// </summary>
    /// <typeparam name="TInput">Входной тип данных</typeparam>
    /// <typeparam name="TOutput">Тип результата</typeparam>
    public abstract class Job<TInput, TOutput> : IJob<TInput, TOutput>
    {
        /// <summary>
        /// Описание работы для вывода на экран
        /// </summary>
        public virtual string Description => $"{GetType().Name}";

        /// <summary>
        /// Показывает, следует ли автоматически завершать работу.
        /// Аналогично установке связи <see cref="JobLink{T}.SingleMessageOptions"/>
        /// </summary>
        protected virtual bool Autocomplete { get; } = true;

        /// <summary>
        /// Делегат для выполнения обработки задания
        /// </summary>
        protected Func<TOutput> Process { get; set; }

        /// <summary>
        /// Делегат для выполнения подготовительного этапа работы
        /// </summary>
        protected Action<TInput> Prepare { get; set; }

        /// <summary>
        /// Логгер для работы
        /// </summary>
        protected ILogger Log { get; private set; }

        /// <summary>
        /// Точка автоматической установки значения логгера
        /// </summary>
        public Holder<ILogger> LogHolder { set { Log = value.Value; } }

        /// <summary>
        /// Обрабатывает автозавершение работы при ее успешном выполнении
        /// </summary>
        /// <param name="output">Результат, полученный в ходе выполнения работы</param>
        protected virtual void ProcessAutocompletion(TOutput output)
        {
            if (Autocomplete)
            {
                (this as IDataflowBlock).Complete();
            }
        }

        /// <summary>
        /// Производит единичную попытку выполнить действие работы,
        /// выполняет подготовительный и непосредственный этапы работы
        /// </summary>
        /// <param name="input">Входные данные для подготовительного этапа</param>
        /// <returns></returns>
        internal virtual TOutput ExecuteInternal(TInput input)
        {
            Prepare?.Invoke(input);
            Log.Warn(Description);

            return Process.Invoke();
        }

        /// <summary>
        /// Производит выполнение работы в соответствии с ее параметрами
        /// </summary>
        /// <param name="input">Входные данные для подготовительного этапа</param>
        /// <returns></returns>
        public virtual TOutput Execute(TInput input)
        {
            var result = ExecuteInternal(input);
            ProcessAutocompletion(result);

            return result;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        protected void Fault(Exception exception)
            => TargetBlock.Fault(exception);

        void IDataflowBlock.Fault(Exception exception)
            => Fault(exception);

        #endregion

        #region ISourceBlock<TResult>

        /// <summary>
        /// Блок, обрабатывающий данные и передающий результат
        /// </summary>
        protected abstract ISourceBlock<TOutput> SourceBlock { get; }

        /// <summary>
        /// Изменяет состояние опций связи работ
        /// </summary>
        /// <param name="linkOptions">Опции связи работ</param>
        /// <returns>Уcтанавливает <see cref="DataflowLinkOptions.PropagateCompletion"/> в значение true</returns>
        protected virtual DataflowLinkOptions ModifyLinkOptions(DataflowLinkOptions linkOptions)
        {
            linkOptions.PropagateCompletion = true;

            return linkOptions;
        }

        public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions)
            => SourceBlock.LinkTo(target, ModifyLinkOptions(linkOptions));

        TOutput ISourceBlock<TOutput>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
            => SourceBlock.ConsumeMessage(messageHeader, target, out messageConsumed);

        bool ISourceBlock<TOutput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
            => SourceBlock.ReserveMessage(messageHeader, target);

        void ISourceBlock<TOutput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
            => SourceBlock.ReleaseReservation(messageHeader, target);

        #endregion
    }
}
