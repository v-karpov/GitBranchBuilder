using System;
using System.Collections.Generic;
using System.Linq;

namespace GitBranchBuilder.Core
{
    /// <summary>
    /// Объединитель веток по умолчанию
    /// </summary>
    public class DefaultBranchCombiner : IBranchCombiner
    {
        /// <summary>
        /// Объединяет ветки в одну на основе логики идентификаторов
        /// </summary>
        /// <param name="sourceBranches">Перечисление веток, которые требуется объединить в одну</param>
        /// <returns></returns>
        public BranchInfo Combine(IEnumerable<BranchInfo> sourceBranches)
        {
            var branchDate = DateTime.Now
                .AddHours(1.0)
                .ToString("ddMMyy_HH");

            var combinedIds = string.Join("_", sourceBranches.Select(x => x.Id));
            var targetBranchName = $"{combinedIds}_{branchDate}";

            return new BranchInfo(targetBranchName, combinedIds);
        }
    }
}
