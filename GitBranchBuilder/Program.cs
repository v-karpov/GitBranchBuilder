using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchBuilder
{
    class Program
    {
        static readonly char[] branchNameSplitters = new[] { '-', '_' };

        static string GetNumber(string branchName)
        {
            var parts = branchName.Split(branchNameSplitters);

            if (parts.Length > 1)
            {
                var start = parts[0].ToUpperInvariant();

                if (start == "URCC" || start == "CRC")
                {
                    return parts[1].PadLeft(3, '0');
                }

                return "unknown project name";
            }

            return string.Empty;
        }


        static void Main(string[] args)
        {
            IBranchDataSource source = new GitlabBranchSource();

            var branches = source.GetBranches("Locko");
            var numbers = branches.Select(branch => GetNumber(branch.Name));

            var branchDate = DateTime.Now.AddHours(1.0);
            var targetBranchName = $"{string.Join("_", numbers)}_{branchDate.ToString("ddMMyy_HH")}";

            var repo = new Repository(@"C:\Git\Locko");
            var branchNames = branches.Select(x => $"origin/{x.Name}").ToHashSet();

            var remote = repo.Network.Remotes["origin"];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

            //repo.Network.Fetch(remote.Url, refSpecs);

            //Commands.Fetch(repo, remote.Name, refSpecs, null, "fetch for PAROVOZ");

            var developTip = repo.Branches
                .Where(x => x.IsRemote && x.FriendlyName == "origin/develop")
                .First().Tip;

            var remoteBranches = repo.Branches.Where(x => branchNames.Contains(x.FriendlyName));
            var tempBranch = repo.Branches.Where(x => x.FriendlyName == targetBranchName).FirstOrDefault() ?? repo.CreateBranch(targetBranchName, developTip);

            Commands.Checkout(repo, tempBranch);

            var mergeSignature = new Signature("karpov", "karpov@elewise.com", DateTimeOffset.Now);

            foreach (var sourceBranch in remoteBranches)
            {
                var result = repo.Merge(sourceBranch, mergeSignature);

                if (result.Status == MergeStatus.Conflicts)
                {
                    Console.WriteLine($"АТАТА КОНФЛИКТЭ АЖ {repo.Index.Conflicts.Count()} штукас");
                }
            }

            Console.ReadKey();

        }
    }
}
