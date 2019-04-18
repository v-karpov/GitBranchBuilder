using LibGit2Sharp;

namespace GitBranchBuilder
{
    public interface ICredentialsProvider
    {
        Credentials GetCredentials(string url, string usernameFromUrl, SupportedCredentialTypes types);
    }
}
