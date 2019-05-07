using GitBranchBuilder.Components;
using NGitLab;
using NGitLab.Models;

namespace GitBranchBuilder.Providers.Gitlab
{
    public class RepositoryClientProvider : Provider<IRepositoryClient>
    {
        public RepositoryClientProvider(
            Holder<GitLabClient> client,
            Holder<Project> project)
        {
            ValueGetter = () =>
                client.Value.GetRepository(project.Value.Id);
        }
    }
}
