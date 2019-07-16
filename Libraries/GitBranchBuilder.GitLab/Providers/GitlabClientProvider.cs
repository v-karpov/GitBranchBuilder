using GitBranchBuilder.Components.Holders.Specific;
using NGitLab;

namespace GitBranchBuilder.Providers.Gitlab
{
    /// <summary>
    /// 
    /// </summary>
    public class GitlabClientProvider : Provider<GitLabClient>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public GitlabClientProvider(ConfigurationHolder config)
        {
            ValueGetter = () => GitLabClient.Connect(
                hostUrl: config["GitLab", "HostUrl"],
                apiToken: config["GitLab", "ApiToken"]
            );
        }
    }
}
