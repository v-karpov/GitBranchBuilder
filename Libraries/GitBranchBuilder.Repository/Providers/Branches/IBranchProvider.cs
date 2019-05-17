using LibGit2Sharp;

namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Интерфейс провайдера веток по <see cref="BranchInfo"/>
    /// </summary>
    public interface IBranchProvider : IProvider
    {
        /// <summary>
        /// Возвращает ветку по ее <see cref="BranchInfo"/>
        /// </summary>
        /// <param name="info">Информация о ветке</param>
        /// <returns></returns>
        Branch GetBranch(BranchInfo info);
    }
}
