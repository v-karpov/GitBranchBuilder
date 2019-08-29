using System.Threading.Tasks.Dataflow;

using CSharpFunctionalExtensions;

using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines.Configarable
{
    /// <summary>
    /// Абстрактный конфигуратор конвейера работ
    /// </summary>
    public abstract class PipelineConfigurator : PipelineConfigurator<Result>
    {

    }

    /// <summary>
    /// Абстрактный конфигуратор конвейера работ
    /// </summary>
    /// <typeparam name="TJob">Тип работ, выполняемых на конвейере</typeparam>
    /// <typeparam name="TResult">Тип результата конвейера</typeparam>
    public abstract class PipelineConfigurator<TResult> : PipelineConfigurator<Result, TResult>
        where TResult : IResult
    {

    }

    /// <summary>
    /// Абстрактный конфигуратор конвейера работ
    /// </summary>
    /// <typeparam name="TJob">Тип работ, выполняемых на конвейере</typeparam>
    /// <typeparam name="TIn">Тип входных данных конвейера</typeparam>
    /// <typeparam name="TResult">Тип результата конвейера</typeparam>
    public abstract class PipelineConfigurator<TIn, TResult> : IPipelineConfigurator<TIn, TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Начальный блок конвейера
        /// </summary>
        public ITargetBlock<TIn> Start { get; protected set; }

        /// <summary>
        /// Функция конфигурации конвейера, возвращающая задачу,
        /// содержающую результат его выполнения
        /// </summary>
        public ResultConfigurator<TIn, TResult> ConfigureResult { get; protected set; }

        /// <summary>
        /// Создает широковещательный блок, рассылающий входные данные на все указанные работы
        /// </summary>
        /// <param name="jobs">Список работ, куда требуется разослать данные</param>
        /// <returns></returns>
        protected BroadcastBlock<TIn> Broadcast(params IJob<TIn>[] jobs)
        {
            var broadcast = new BroadcastBlock<TIn>(x => x);

            foreach (var job in jobs)
            {
                broadcast.LinkTo(job);
            }

            return broadcast;
        }
    }
}
