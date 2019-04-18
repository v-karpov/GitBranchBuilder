using NGitLab;

namespace GitBranchBuilder
{
    public class GitlabClientProvider : IGitlabClientProvider
    {
        public GitLabClient Client { get; }

        public GitlabClientProvider(IConfigurationProvider configurationProvider)
        {
            var config = configurationProvider.Configuration["GitLab"];

            Client = GitLabClient.Connect(
                hostUrl: config["HostUrl"].StringValue,
                apiToken: config["ApiToken"].StringValue
            );
        }
    }
}
