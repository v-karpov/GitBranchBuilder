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
        static Dictionary<string, string> Labels = new Dictionary<string, string>
        {
            ["fix"] = "f",
            ["lb"] = "lb",
            ["integration"] = "i"
        };

        private string ProcessLabels(BranchInfo info)
        {
            if (info.Tag is string[] labels)
            {
                var otherLabels = labels
                    .Select(x => x.ToLower())
                    .Where(x => x != "chu-chu")
                    .Select(x => (Labels.TryGetValue(x, out var value), value ?? x));

                return $"{info.Id}{otherLabels.FirstOrDefault(x => x.Item1).Item2}";
            }

            return info.Id;
        }

        /// <summary>
        /// Объединяет ветки в одну на основе логики идентификаторов
        /// </summary>
        /// <param name="sourceBranches">Перечисление веток, которые требуется объединить в одну</param>
        /// <returns></returns>
        public BranchInfo Combine(IEnumerable<BranchInfo> sourceBranches)
        {
            var branchDate = DateTime.Now
                .ToString("yyMMdd_HHmm");

            var combinedIds = string.Join("_", sourceBranches.Select(ProcessLabels));
            var targetBranchName = $"{branchDate}_{combinedIds}";

            return new BranchInfo(targetBranchName, combinedIds);
        }
    }
}
