using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GitBranchBuilder.Pipelines;

namespace GitBranchBuilder.Jobs
{

    /// <summary>
    /// Работа, являющаяся концом конвейера
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    public interface IFinishJob<TInput> : IJob<TInput>, ISourceBlock<PipelineResult>
    {
        PipelineResult Result { get; }
    }

    /// <summary>
    /// Абстрактный класс, описывающий блок работы, являющийся финальным в своей ветви конвейера
    /// </summary>
    /// <typeparam name="TInput">Входной тип данных</typeparam>
    public abstract class FinishJob<TInput> :
        PropagationJob<TInput, PipelineResult>, IFinishJob<TInput>
    {
        /// <summary>
        /// Результат выполнения данной ветви конвейера
        /// </summary>
        public PipelineResult Result { get; protected set; }

        /// <summary>
        /// Блок, устанавливающий результат
        /// </summary>
        protected ActionBlock<PipelineResult> ResultSetter { get; set; }

        /// <summary>
        /// Задача, сигнализирующая о завершении обработки данных блоком
        /// </summary>
        public override Task Completion => ResultSetter.Completion;

        /// <summary>
        /// Делегат, производщий обработку без выдачи сообщений и результата
        /// </summary>
        protected virtual Action ProcessQuietly { get; set; }

        /// <summary>
        /// Переопределенный метод, возвращающий <see cref="PipelineResult.Empty"/> в случае,
        /// если обработчик не установлен или не возвращает результат
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override PipelineResult Execute(TInput input)
        {
            Prepare?.Invoke(input);

            Console.WriteLine($"{Description}");

            if (ProcessQuietly != null)
            {
                ProcessQuietly();
            }
            else if (Process != null)
            {
                return Process();
            }

            return PipelineResult.Empty;
        }

        public FinishJob()
        {
            ResultSetter = new ActionBlock<PipelineResult>(x => Result = x);
            DataflowBlock.LinkTo(this, ResultSetter);
        }
    }
}
