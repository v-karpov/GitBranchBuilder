using GitBranchBuilder.Components;
using NGitLab.Models;

namespace GitBranchBuilder.Providers.Gitlab
{
    /// <summary>
    /// Провайдер ветки по умолчанию на основе <see cref="Project"/>
    /// </summary>
    public class GitlabDefaultBranchProvider : Provider<DefaultBranchInfo>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="gitlabProject">Проект в GitLab</param>
        /// <param name="remoteBranchProvider">Провайдер удаленных ветвей</param>
        /// <param name="branchInfoProvider">Провайдер информации о ветвях</param>
        public GitlabDefaultBranchProvider(
            Holder<Project> gitlabProject,
            IRemoteBranchProvider remoteBranchProvider,
            IBranchInfoProvider branchInfoProvider)
        {
            ValueGetter = () =>
            {
                var project = gitlabProject.Value;

                var defaultBranchInfo = project == null
                    ? BranchInfo.Develop
                    : branchInfoProvider.GetBranchInfo(project.DefaultBranch);

                return new DefaultBranchInfo(defaultBranchInfo, remoteBranchProvider);             
            };
        }
    }
}