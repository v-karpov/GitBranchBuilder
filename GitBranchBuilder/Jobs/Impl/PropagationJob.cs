using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Абстрактный класс, описывающий блок работы, выполняющий передачу своего результата
    /// </summary>
    /// <typeparam name="TInput">Входной тип данных</typeparam>
    /// <typeparam name="TOutput">Тип результата</typeparam>
    public class PropagationJob<TInput, TOutput> : Job<TInput, TOutput>, IPropagatorBlock<TInput, TOutput>
    {
        /// <summary>
        /// Блок преобразования, обернутый для использования работой
        /// </summary>
        protected virtual IPropagatorBlock<TInput, TOutput> PropagatorBlock { get; }

        /// <summary>
        /// Блок, получающий данные на вход и обрабатывающий их
        /// </summary>
        protected override ITargetBlock<TInput> TargetBlock => PropagatorBlock;

        /// <summary>
        /// Блок, обрабатывающий данные и передающий результат
        /// </summary>
        protected override ISourceBlock<TOutput> SourceBlock => PropagatorBlock;

        /// <summary>
        /// Функция, возвращающая заполненный блок <see cref="IPropagatorBlock{TInput, TOutput}"/>
        /// </summary>
        protected virtual IPropagatorBlock<TInput, TOutput> GetPropagatorBlock() => default;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PropagationJob(IPropagatorBlock<TInput, TOutput> propagatorBlock) : base()
            => PropagatorBlock = propagatorBlock;

        public PropagationJob() : this(null)
           => PropagatorBlock = GetPropagatorBlock() ?? new TransformBlock<TInput, TOutput>(Execute);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class PropagationJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="propagatorBlock"></param>
        /// <returns></returns>
        public static PropagationJob<TInput, TOutput> FromBlock<TInput, TOutput>(IPropagatorBlock<TInput, TOutput> propagatorBlock)
            => new PropagationJob<TInput, TOutput>(propagatorBlock);
    }
}
