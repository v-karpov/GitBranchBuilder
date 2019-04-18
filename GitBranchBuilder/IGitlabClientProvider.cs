using NGitLab;

namespace GitBranchBuilder
{
    public interface IGitlabClientProvider
    {
        GitLabClient Client { get; }
    }
}