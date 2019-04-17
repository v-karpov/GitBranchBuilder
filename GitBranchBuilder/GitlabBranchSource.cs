using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGitLab;
using NGitLab.Models;

namespace GitBranchBuilder
{
    public class GitlabBranchSource : IBranchDataSource
    {
        public static readonly HashSet<string> PickedLabels = new HashSet<string> { "chu-chu" };

        public IEnumerable<Branch> GetBranches(string projectName)
        {
            var client = GitLabClient.Connect(
                hostUrl: "http://vnd-git.sale.elewise.com",
                apiToken: "sWe94u2X-h9SHEs8feGf");

            var proj = client.Projects
                .Membership()
                .Where(x => x.Name == projectName)
                .FirstOrDefault();

            var repo = client.GetRepository(proj.Id);

            if (proj == null)
                return Enumerable.Empty<Branch>();

            var openedMergeRequests = client
                .GetMergeRequest(proj.Id)
                .AllInState(MergeRequestState.opened);

            var pickedMergeRequests = openedMergeRequests
                .Where(x => x.Labels.Any(PickedLabels.Contains));
            var pickedBranches = pickedMergeRequests
                .Select(x => repo.Branches.Get(x.SourceBranch));

            return pickedBranches;
        }
    }
}
