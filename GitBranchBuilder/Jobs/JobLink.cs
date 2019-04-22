using System;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Класс, описывающий связь работ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JobLink<T> : IDisposable
    {
        /// <summary>
        /// Целевая работа, с которой проивзедена связь
        public IJob<T> Target { get; }

        /// <summary>
        /// Объект <see cref="IDisposable"/>, при очистке которого связь разраывается
        /// </summary>
        public IDisposable Link { get; }

        /// <summary>
        /// Метод очистики и разрыва связи
        /// </summary>
        public void Dispose()
            => Link.Dispose();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="target"></param>
        /// <param name="link"></param>
        public JobLink(IJob<T> target, IDisposable link)
        {
            Target = target;
            Link = link;
        }

        /// <summary>
        /// Конструктор, выполняющий связь между блоками работ
        /// </summary>
        /// <param name="source">Источник данных</param>
        /// <param name="target">Работа, с которой требуется выполнить связь</param>
        public JobLink(ISourceBlock<T> source, IJob<T> target)
            : this(target, DataflowBlock.LinkTo(source, target))
        {

        }
    }
}
