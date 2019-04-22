using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Библиотека методов расширения для <see cref="Job{TInput, TOutput}"/>
    /// </summary>
    public static class JobExtensions
    {
        /// <summary>
        /// Связывает блок исходных данных с объектом работы 
        /// </summary>
        /// <typeparam name="TInput">Тип входных данных</typeparam>
        /// <param name="source">Источник данных</param>
        /// <param name="target">Целевая работа</param>
        /// <returns></returns>
        public static JobLink<TInput> LinkTo<TInput>(this ISourceBlock<TInput> source, IJob<TInput> target) 
            => new JobLink<TInput>(source, target);
    }
}
