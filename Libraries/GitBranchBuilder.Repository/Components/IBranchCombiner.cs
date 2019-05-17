using System.Collections.Generic;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Интерфейс компонента, способного объединять ветки в одну
    /// </summary>
    public interface IBranchCombiner : IComponent
    {
        /// <summary>
        /// Объединяет несколько описаний веток в одно описание по определенному правилу
        /// </summary>
        /// <param name="sourceBranches">Перчисление объединяемых веток</param>
        /// <returns></returns>
        BranchInfo Combine(IEnumerable<BranchInfo> sourceBranches);
    }
}
