using System;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Интерфейс, описывающий блок работы
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

    /// <summary>
    /// Интерфейс, описывающий блок работы
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных блока работы</typeparam>
    /// <typeparam name="TOutput">Тип выходных данных блока работы</typeparam>
    public interface IJob<in TInput, out TOutput> : IJob<TInput>, IPropagatorBlock<TInput, TOutput>
    {
        /// <summary>
        /// Производит выполнение работы в соответствии с ее параметрами
        /// </summary>
        /// <param name="input">Входные данные для подготовительного этапа</param>
        TOutput Execute(TInput input);
    }
}
