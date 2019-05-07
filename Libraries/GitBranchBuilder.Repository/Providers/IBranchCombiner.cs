using System.Collections.Generic;

namespace GitBranchBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBranchCombiner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceBranches"></param>
        /// <returns></returns>
        BranchInfo Combine(IEnumerable<BranchInfo> sourceBranches);
    }
}
