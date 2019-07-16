using System;
using System.Threading.Tasks.Dataflow;

using GitBranchBuilder.Jobs;

namespace GitBranchBuilder.Pipelines
{
    /// <summary>
    /// Набор мтеодов расширения для объектов <see cref="IPipeline"/>
    /// </summary>
    public static class PipelineExtensions
    {
        /// <summary>
        /// Для заданного блока создает задачу преобразования с указанной функцией преобразования
        /// </summary>
        /// <typeparam name="TIn">Тип входных данных задачи преобразования</typeparam>
        /// <typeparam name="TOut">Тип выходных данных задачи преобразования</typeparam>
        /// <param name="sourceBlock">Блок, являющийся источником данных типа <typeparamref name="TIn"/></param>
        /// <param name="pipeline">Конвейер, для которого производится создание задачи</param>
        /// <param name="transform">Функция преобразования объекта</param>
        /// <returns></returns>
        public static PropagationJob<TIn, TOut> Transform<TIn, TOut>(
            this IPipeline pipeline,
            ISourceBlock<TIn> sourceBlock,
            Func<TIn, TOut> transform)
        {
            var transformer = new TransformBlock<TIn, TOut>(transform, pipeline.ExecutionOptions);
            sourceBlock.LinkTo(transformer, pipeline.LinkOptions);

            return PropagationJob.FromBlock(transformer);
        }

        /// <summary>
        /// Для двух заданных блоков создает задачу, объединяющую их 
        /// входные данные и возвращющую агрегатный результат
        /// </summary>
        /// <typeparam name="T1">Тип данных блока <paramref name="source1"/></typeparam>
        /// <typeparam name="T2">Тип данных блока <paramref name="source2"/></typeparam>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="source1">Блок, являющийся источником данных типа <typeparamref name="T1"/></param>
        /// <param name="source2">Блок, являющийся источником данных типа <typeparamref name="T2"/></param>
        /// <param name="pipeline">Конвейер, для которого производится создание задачи</param>
        /// <param name="transform">Функция преобразования агрегатного результата объединения</param>
        /// <returns>Задача по преобразованию объединенного входа двух блоков</returns>
        public static PropagationJob<Tuple<T1, T2>, T> Join<T1, T2, T>(
            this IPipeline pipeline,
            ISourceBlock<T1> source1,
            ISourceBlock<T2> source2,
            Func<Tuple<T1, T2>, T> transform)
        {
            var joint = new JoinBlock<T1, T2>(pipeline.GroupingOptions);

            source1.LinkTo(joint.Target1, pipeline.LinkOptions);
            source2.LinkTo(joint.Target2, pipeline.LinkOptions);

            return Transform(pipeline, joint, transform);
        }

        /// <summary>
        /// Для двух заданных блоков создает задачу, объединяющую их 
        /// входные данные и возвращющую агрегатный результат
        /// </summary>
        /// <typeparam name="T1">Тип данных блока <paramref name="source1"/></typeparam>
        /// <typeparam name="T2">Тип данных блока <paramref name="source2"/></typeparam>
        /// <typeparam name="T3">Тип данных блока <paramref name="source3"/></typeparam>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="source1">Блок, являющийся источником данных типа <typeparamref name="T1"/></param>
        /// <param name="source2">Блок, являющийся источником данных типа <typeparamref name="T2"/></param>
        /// <param name="source3">Блок, являющийся источником данных типа <typeparamref name="T3"/></param>
        /// <param name="pipeline">Конвейер, для которого производится создание задачи</param>
        /// <param name="transform">Функция преобразования агрегатного результата объединения</param>
        /// <returns>Задача по преобразованию объединенного входа трех блоков</returns>
        public static PropagationJob<Tuple<T1, T2, T3>, T> Join<T1, T2, T3, T>(
            this IPipeline pipeline,
            ISourceBlock<T1> source1,
            ISourceBlock<T2> source2,
            ISourceBlock<T3> source3,
            Func<Tuple<T1, T2, T3>, T> transform)
        {
            var joint = new JoinBlock<T1, T2, T3>(pipeline.GroupingOptions);

            source1.LinkTo(joint.Target1, pipeline.LinkOptions);
            source2.LinkTo(joint.Target2, pipeline.LinkOptions);
            source3.LinkTo(joint.Target3, pipeline.LinkOptions);

            return Transform(pipeline, joint, transform);
        }
    }
}
