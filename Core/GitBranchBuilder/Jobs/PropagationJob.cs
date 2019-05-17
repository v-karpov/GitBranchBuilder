using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Абстрактный класс, описывающий блок работы, выполняющий передачу своего результата
    /// </summary>
    /// <typeparam name="TInput">Входной тип данных</typeparam>
    /// <typeparam name="TOutput">Тип результата</typeparam>
    public abstract class PropagationJob<TInput, TOutput> : Job<TInput, TOutput>, IPropagatorBlock<TInput, TOutput>
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
        protected PropagationJob(IPropagatorBlock<TInput, TOutput> propagatorBlock) : base()
            => PropagatorBlock = propagatorBlock;

        protected PropagationJob() : this(null)
           => PropagatorBlock = GetPropagatorBlock() ?? new TransformBlock<TInput, TOutput>(Execute);
    }

    /// <summary>
    /// Вспомогательные методы для работы с <see cref="PropagationJob{TInput, TOutput}"/>
    /// </summary>
    public static class PropagationJob
    {
        /// <summary>
        /// Реализация класса <see cref="PropagationJob{TInput, TOutput}"/> для создания независимых работ
        /// </summary>
        /// <typeparam name="TInput">Входной тип данных</typeparam>
        /// <typeparam name="TOutput">Тип результата</typeparam>
        private class PropagationJobImpl<TInput, TOutput> : PropagationJob<TInput, TOutput>
        {
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            public PropagationJobImpl(IPropagatorBlock<TInput, TOutput> propagatorBlock) : base(propagatorBlock)
            {
            }
        }

        /// <summary>
        /// Создает <see cref="PropagationJob{TInput, TOutput}"/> из существубщего блока трансформации данных <see cref="IPropagatorBlock{TInput, TOutput}"/>
        /// </summary>
        /// <typeparam name="TInput">Входной тип данных</typeparam>
        /// <typeparam name="TOutput">Тип результата</typeparam>
        /// <param name="propagatorBlock">Блок, преобразующий данные из <typeparamref name="TInput"/> в <typeparamref name="TOutput"/></param>
        /// <returns></returns>
        public static PropagationJob<TInput, TOutput> FromBlock<TInput, TOutput>(IPropagatorBlock<TInput, TOutput> propagatorBlock)
            => new PropagationJobImpl<TInput, TOutput>(propagatorBlock);
    }
}
