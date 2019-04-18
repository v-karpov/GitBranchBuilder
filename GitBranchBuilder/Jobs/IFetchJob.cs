using LibGit2Sharp;

namespace GitBranchBuilder.Jobs.Pipelines.Merge
{
    public interface IFetchJob : IJob
    {
        Remote Remote { get; }

        Repository Repository { get; }
    }
}
