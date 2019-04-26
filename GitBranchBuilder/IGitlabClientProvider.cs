using System;
using NGitLab;

namespace GitBranchBuilder
{
    public interface IGitlabClientProvider
    {
        Lazy<GitLabClient> Client { get; }
    }
}