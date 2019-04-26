using System;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJob : IDisposable
    {
        /// <summary>
        /// Описание работы, понятное для пользователя
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Интерфейс, описывающий блок работы
    /// </summary>
    /// <typeparam name="TIn">Тип входных данных блока работы</typeparam>
    public interface IJob<in TIn> : IJob, ITargetBlock<TIn>
    {
        
    }

    public interface IJob<in TIn, out TOut> : IJob<TIn>, ISourceBlock<TOut>
    {

    }
}
