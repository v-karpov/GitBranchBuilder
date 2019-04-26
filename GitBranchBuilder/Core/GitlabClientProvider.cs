using System;
using NGitLab;

namespace GitBranchBuilder
{
    public class GitlabClientProvider : IGitlabClientProvider
    {
        public Lazy<GitLabClient> Client { get; }

        public GitlabClientProvider(IConfigurationProvider configurationProvider)
        {
            var config = configurationProvider.Configuration["GitLab"];

            Client = new Lazy<GitLabClient>(() => GitLabClient.Connect(
                hostUrl: config["HostUrl"].StringValue,
                apiToken: config["ApiToken"].StringValue
            ));
        }
    }
}
