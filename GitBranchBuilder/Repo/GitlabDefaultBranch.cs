using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class GitlabDefaultBranch : IDefaultBranch
    {
        public Branch Branch => BranchLoader.Value;

        protected Lazy<Branch> BranchLoader { get; }

        public GitlabDefaultBranch(
            IGitlabClientProvider gitlabClientProvider,
            IRemoteBranchProvider remoteBranchProvider,
            IBranchInfoProvider branchInfoProvider,
            IConfigurationProvider configurationProvider)
        {
            var config = configurationProvider.Configuration["GitLab"];
            var projectName = config["Project"].StringValue;

            BranchLoader = new Lazy<Branch>(() =>
            {
                var client = gitlabClientProvider.Client.Value;
                var project = client.Projects
                    .Accessible()
                    .FirstOrDefault(x => x.Name == projectName);

                var defaultBranch = project == null
                    ? BranchInfo.Develop
                    : branchInfoProvider.GetBranchInfo(project.DefaultBranch);

                return remoteBranchProvider.GetBranch(defaultBranch);
            });
        }
    }
}
