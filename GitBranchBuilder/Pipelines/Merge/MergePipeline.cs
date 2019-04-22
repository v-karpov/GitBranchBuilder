using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Autofac;
using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines.Merge
{
    public interface IMergeJob { }

    public interface IMergeStartJob : IMergeJob, IStartJob { }

    public interface IMergeFinishJob : IMergeJob, IFinishJob<Data.BuildJobResult> { }

    public interface IMergePipeline : IPipeline { }

    /// <summary>
    /// Конвейер, выполняющий слияние веток
    /// </summary>
    public class MergePipeline : IMergePipeline
    {
        /// <summary>
        /// Стартовое задание
        /// </summary>
        public CombinedStartJob Start { get; set; }

        public Task Completion { get; set; }

        protected Func<PipelineResult> FindResult { get; set; }

        /// <summary>
        /// Опции выполнения заданий обработки
        /// </summary>
        public ExecutionDataflowBlockOptions ExecutionOptions { get; private set; }

        /// <summary>
        /// Опции группировки в заданиях
        /// </summary>
        public GroupingDataflowBlockOptions GroupingOptions { get; private set; }

        /// <summary>
        /// Очищает ресурсы данного объекта
        /// </summary>
        public void Dispose() { Start.Dispose(); }

        /// <summary>
        /// Подготоваливает конвейер к запуску
        /// </summary>
        /// <param name="container">Контейнер зависимостей Autofac</param>
        public void Prepare(IContainer container)
        {
            var startJobs = container.Resolve<IEnumerable<IMergeStartJob>>();
            var finishJobs = container.Resolve<IEnumerable<IMergeFinishJob>>();

            Start = new CombinedStartJob(startJobs);
            Completion = finishJobs.Select(x => x.Completion).First();

            FindResult = () => finishJobs
                .Where(job => job.Completion.IsCompleted)
                .First()
                .Result;
        }

        public async Task<PipelineResult> Run(PipelineOptions options)
        {
            Start.Post(options);
            await Completion;

            return FindResult?.Invoke() ?? PipelineResult.Empty;
        }
    }
}
