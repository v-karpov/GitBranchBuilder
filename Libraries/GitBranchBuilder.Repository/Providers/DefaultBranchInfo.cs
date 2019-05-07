using GitBranchBuilder.Providers;
using LibGit2Sharp;

namespace GitBranchBuilder
{
    /// <summary>
    /// Информация о ветви по умолчанию
    /// </summary>
    public class DefaultBranchInfo
    {
        /// <summary>
        /// Ветвь по умолчанию
        /// </summary>
        public Branch Branch => BranchProvider.GetBranch(BranchInfo);

        /// <summary>
        /// Информация о ветви, по которой будет получена сама ветвь
        /// </summary>
        public BranchInfo BranchInfo { get; }

        /// <summary>
        /// Провайдер ветвей, при помощи которого будет получена ветвь
        /// </summary>
        public IBranchProvider BranchProvider { get; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="info">Информация о ветви</param>
        /// <param name="branchProvider">Провайдер ветви</param>
        public DefaultBranchInfo(BranchInfo info, IBranchProvider branchProvider)
        {
            BranchInfo = info;
            BranchProvider = branchProvider;
        }
    }
}