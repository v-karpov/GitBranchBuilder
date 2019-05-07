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
        /// Опции связи задач по умолчанию
        /// </summary>
        public static DataflowLinkOptions DefaultOptions { get; } 
            = new DataflowLinkOptions();

        /// <summary>
        /// Опции связи задач, описывающие обработку единичного сообщения
        /// и прекращения принятия последующих
        /// </summary>
        public static DataflowLinkOptions SingleMessageOptions { get; } 
            = new DataflowLinkOptions
            {
                MaxMessages = 1,
                PropagateCompletion = true
            };

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
        public JobLink(ISourceBlock<T> source, IJob<T> target, DataflowLinkOptions linkOptions)
            : this(target, source.LinkTo(target, linkOptions))
        {

        }

        /// <summary>
        /// Конструктор, выполняющий связь между блоками работ
        /// </summary>
        /// <param name="source">Источник данных</param>
        /// <param name="target">Работа, с которой требуется выполнить связь</param>
        public JobLink(ISourceBlock<T> source, IJob<T> target)
            : this(source, target, DefaultOptions)
        {

        }
    }
}
