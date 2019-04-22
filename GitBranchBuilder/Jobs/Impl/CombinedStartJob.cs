using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GitBranchBuilder.Pipelines;
using MoreLinq;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Объект совмещенного старта нескольких работ
    /// </summary>
    public class CombinedStartJob : IJob<PipelineOptions>, IStartJob
    {
        /// <summary>
        /// Описание работы
        /// </summary>
        public string Description 
            => $"Combination of jobs: \r\n{CombinedDescription}";

        /// <summary>
        /// Совмещенное описание дочерних работ
        /// </summary>
        protected string CombinedDescription
            => string.Join("\r\n", "\t" + Jobs.Select(x => x.Description));

        /// <summary>
        /// Список дочерних работ
        /// </summary>
        public IReadOnlyCollection<IStartJob> Jobs { get; }

        /// <summary>
        /// Список связей с дочерними работами
        /// </summary>
        public IReadOnlyCollection<JobLink<PipelineOptions>> Links { get; }

        /// <summary>
        /// Блок, рассылающий данные дочерним работам
        /// </summary>
        protected ITargetBlock<PipelineOptions> Broadcast { get; }

        /// <summary>
        /// Освобождает все ресурсы, которыми владеет данный объект
        /// </summary>
        public virtual void Dispose() => Links.ForEach(x => x.Dispose());

        #region [ITargetBlock<PipelineStartOptions>]

        Task IDataflowBlock.Completion => Broadcast.Completion;
        
        DataflowMessageStatus ITargetBlock<PipelineOptions>.OfferMessage(DataflowMessageHeader messageHeader,
                                                                              PipelineOptions messageValue,
                                                                              ISourceBlock<PipelineOptions> source,
                                                                              bool consumeToAccept)
            => Broadcast.OfferMessage(messageHeader, messageValue, source, consumeToAccept);

        void IDataflowBlock.Complete() => Broadcast.Complete();

        void IDataflowBlock.Fault(Exception exception) => Broadcast.Fault(exception);

        #endregion

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="pipelineStarts"></param>
        public CombinedStartJob(IEnumerable<IStartJob> pipelineStarts)
        {
            var broadcast = new BroadcastBlock<PipelineOptions>(x => x);

            Links = pipelineStarts.Select(broadcast.LinkTo).ToList();
            Broadcast = broadcast;
        }
    }
}
