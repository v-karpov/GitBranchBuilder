using System.Linq;
using GitBranchBuilder.Components;
using LibGit2Sharp;

using IRemoteBranchLookup = System.ValueTuple<LibGit2Sharp.Remote, System.Linq.ILookup<string, LibGit2Sharp.Branch>>;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRemoteBranchProvider : IBranchProvider
    {
        Remote Remote { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RemoteBranchProvider : IRemoteBranchProvider
    {
        /// <summary>
        /// 
        /// </summary>
        private class RemoteBranchLookupProvider : Provider<IRemoteBranchLookup>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="repository"></param>
            /// <param name="remoteHolder"></param>
            public RemoteBranchLookupProvider(
                RepositoryHolder repository,
                Holder<Remote> remoteHolder)
            {
                ValueGetter = () =>
                {
                    var branchLookup = repository.Value.Branches
                        .Where(x => x.IsRemote)
                        .ToLookup(x => x.FriendlyName
                            .Replace(remoteHolder.Value.Name + "/", ""));

                    return (remoteHolder.Value, branchLookup);
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Remote Remote => Branches.Value.Item1;

        /// <summary>
        /// 
        /// </summary>
        public Holder<IRemoteBranchLookup> Branches { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public Branch GetBranch(BranchInfo branch)
            => Branches.Value.Item2[branch.Name]
                .SingleOrDefault();
    }
}
