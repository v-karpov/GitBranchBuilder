using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Объект совмещенного старта нескольких работ
    /// </summary>
    public class CombinedStartJob : StartJob<StartOptions>
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
        public IReadOnlyCollection<JobLink<StartOptions>> Links { get; }

        /// <summary>
        /// Блок, рассылающий данные дочерним работам
        /// </summary>
        protected BroadcastBlock<StartOptions> Broadcast { get; }

        /// <summary>
        /// Блок, обрабатывающий данные и передающий результат
        /// </summary>
        protected override ISourceBlock<StartOptions> SourceBlock => Broadcast;

        /// <summary>
        /// Блок, получающий данные на вход и обрабатывающий их
        /// </summary>
        protected override ITargetBlock<StartOptions> TargetBlock => Broadcast;

        /// <summary>
        /// Освобождает все ресурсы, которыми владеет данный объект
        /// </summary>
        public override void Dispose()
        {
            foreach (var link in Links)
                link.Dispose();
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="pipelineStarts"></param>
        public CombinedStartJob(IEnumerable<IStartJob> pipelineStarts)
        {
            var broadcast = new BroadcastBlock<StartOptions>(x => x);

            Links = pipelineStarts.Select(broadcast.LinkSingle).ToList();
            Broadcast = broadcast;
        }
    }
}
