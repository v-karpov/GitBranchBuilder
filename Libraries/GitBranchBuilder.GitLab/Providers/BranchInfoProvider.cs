using System.Collections.Generic;
using System.Linq;

using GitBranchBuilder.Components;

using NGitLab;
using NGitLab.Models;

namespace GitBranchBuilder.Providers.Gitlab
{
    /// <summary>
    /// Провайдер информации о ветвях, предоставляющий перечисление <see cref="BranchInfo"/>
    /// </summary>
    public class BranchInfoProvider : Provider<List<BranchInfo>>
    {
        /// <summary>
        /// Конктруктор по умолчанию для типа <see cref="BranchInfoProvider"/>
        /// </summary>
        /// <param name="configuration">Конфигурация системы</param>
        /// <param name="mergeRequsts">Клиент запросов на слияние в системе GitLab</param>
        /// <param name="branchInfoProvider">Провайдер информации о конкретной ветви</param>
        public BranchInfoProvider(
            ConfigurationHolder configuration,
            Holder<IMergeRequestClient> mergeRequsts,
            IBranchInfoProvider branchInfoProvider)
        {
            ValueGetter = () =>
            {
                var labels = new HashSet<string>(configuration.Array("GitLab", "PickedLabels"));

                var pickedMergeRequests = mergeRequsts.Value
                    .AllInState(MergeRequestState.opened)
                    .Where(x => x.Labels.Any(labels.Contains));

                var branches = pickedMergeRequests
                    .Select(x => branchInfoProvider
                        .GetBranchInfo(x.SourceBranch)
                        .SetTag(x.Labels))
                    .OrderBy(x => x.Id);

                return branches.ToList();
            };
        }
    }
}
