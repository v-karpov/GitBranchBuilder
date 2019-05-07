using GitBranchBuilder.Pipelines;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Работа, являющаяся началом конвейера
    /// </summary>
    public interface IStartJob : IJob<StartOptions> { }

    /// <summary>
    /// Абстракткный класс, описывающий начальный блок работы
    /// </summary>
    /// <typeparam name="TOutput">Тип результата данного блока работы</typeparam>
    public abstract class StartJob<TOutput> :
        PropagationJob<StartOptions, TOutput>, IStartJob
    {

    }
}
