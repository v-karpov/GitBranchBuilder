using System;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Интерфейс, описывающий блок работы
    /// </summary>
    /// <typeparam name="TIn">Тип входных данных блока работы</typeparam>
    public interface IJob<in TIn> : 
        ITargetBlock<TIn>,
        IDisposable
    {
        /// <summary>
        /// Описание работы, понятное для пользователя
        /// </summary>
        string Description { get; }
    }
}
