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
    /// <typeparam name="TInput">Тип входных данных блока работы</typeparam>
    public interface IJob<in TInput> : IJob, ITargetBlock<TInput>
    {
        
    }

    public interface IJob<in TInput, out TOutput> : IJob<TInput>, ISourceBlock<TOutput>
    {
        /// <summary>
        /// Производит выполнение работы в соответствии с ее параметрами
        /// </summary>
        /// <param name="input">Входные данные для подготовительного этапа</param>
        TOutput Execute(TInput input);
    }
}
