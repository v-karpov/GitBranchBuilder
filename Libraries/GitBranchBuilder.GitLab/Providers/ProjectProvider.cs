using System.Linq;

using GitBranchBuilder.Components;

using NGitLab;
using NGitLab.Models;

namespace GitBranchBuilder.Providers.Gitlab
{
    public class ProjectProvider : Provider<Project>
    {
        public ProjectProvider(
            ConfigurationHolder config,
            Holder<GitLabClient> client)
        {
            ValueGetter = () =>
            {
                string projectName = config["GitLab", "Project"];

                return client.Value.Projects
                   .Accessible()
                   .Where(x => x.Name == projectName)
                   .Single();
            };
        }
    }
}
