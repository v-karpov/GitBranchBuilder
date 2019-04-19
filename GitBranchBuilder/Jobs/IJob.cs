using System;
using System.Threading.Tasks;

namespace GitBranchBuilder.Jobs
{
    public interface IJob : IDisposable
    {
        string Description { get; }

        Task Task { get; }
    }
}
