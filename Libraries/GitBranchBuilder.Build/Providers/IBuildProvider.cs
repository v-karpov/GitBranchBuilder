using Microsoft.Build.Framework;

namespace GitBranchBuilder.Providers.Build
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBuildProvider : IProvider<BuildEngineResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        BuildEngineResult GetValue(BuildPath path, ILogger logger);
    }
}
