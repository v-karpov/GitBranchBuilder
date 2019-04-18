using System.Collections.Generic;
using System.Linq;

using NGitLab;
using NGitLab.Models;

namespace GitBranchBuilder
{
    public class GitlabBranchSource : IBranchSource
    {
        public HashSet<string> PickedLabels { get; }

        public IBranchInfoProvider BranchInfoProvider { get; }

        public GitLabClient Client { get; }

        public IEnumerable<BranchInfo> GetBranches(string projectName)
        {
            var proj = Client.Projects
                .Membership()
                .FirstOrDefault(x => x.Name == projectName);

            if (proj == null)
                return Enumerable.Empty<BranchInfo>();

            var openedMergeRequests = Client
                .GetMergeRequest(proj.Id)
                .AllInState(MergeRequestState.opened);

            var pickedMergeRequests = openedMergeRequests
                .Where(x => x.Labels.Any(PickedLabels.Contains));

            var pickedBranches = pickedMergeRequests
                .Select(x => BranchInfoProvider.GetBranchInfo(x.SourceBranch))
                .OrderBy(x => x.Id);

            return pickedBranches;
        }

        public GitlabBranchSource(
            IGitlabClientProvider gitlabClientProvider,
            IBranchInfoProvider branchInfoProvider,
            IConfigurationProvider configurationProvider)
        {
            Client = gitlabClientProvider.Client;
            BranchInfoProvider = branchInfoProvider;

            PickedLabels = configurationProvider
                .Configuration["GitLab"]["PickedLabels"]
                .StringValueArray
                .ToHashSet();
        }
    }
}
