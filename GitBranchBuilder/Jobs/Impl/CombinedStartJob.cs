using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

using GitBranchBuilder.Pipelines;

using MoreLinq;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Объект совмещенного старта нескольких работ
    /// </summary>
    public class CombinedStartJob : StartJob<PipelineOptions>
    {
        /// <summary>
        /// Описание работы
        /// </summary>
        public override string Description 
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
        protected BroadcastBlock<PipelineOptions> Broadcast { get; }

        /// <summary>
        /// Блок, обрабатывающий данные и передающий результат
        /// </summary>
        protected override ISourceBlock<PipelineOptions> SourceBlock => Broadcast;

        /// <summary>
        /// Блок, получающий данные на вход и обрабатывающий их
        /// </summary>
        protected override ITargetBlock<PipelineOptions> TargetBlock => Broadcast;

        /// <summary>
        /// Освобождает все ресурсы, которыми владеет данный объект
        /// </summary>
        public override void Dispose() => Links.ForEach(x => x.Dispose());

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="pipelineStarts"></param>
        public CombinedStartJob(IEnumerable<IStartJob> pipelineStarts)
        {
            var broadcast = new BroadcastBlock<PipelineOptions>(x => x);

            Links = pipelineStarts.Select(broadcast.LinkSingle).ToList();
            Broadcast = broadcast;
        }
    }
}
