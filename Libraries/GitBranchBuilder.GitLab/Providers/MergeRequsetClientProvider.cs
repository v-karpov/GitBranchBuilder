using GitBranchBuilder.Components;
using NGitLab;
using NGitLab.Models;

namespace GitBranchBuilder.Providers.Gitlab
{
    public class MergeRequesetClientProvider : Provider<IMergeRequestClient>
    {
        public MergeRequesetClientProvider(
            Holder<GitLabClient> client,
            Holder<Project> project)
        {
            ValueGetter = () =>
                client.Value.GetMergeRequest(project.Value.Id);
        }
    }
}
