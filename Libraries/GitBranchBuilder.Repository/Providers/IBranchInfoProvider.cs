namespace GitBranchBuilder.Providers
{
    /// <summary>
    /// Интерфейс провайдера информации о ветви на основе ее полного имени
    /// </summary>
    public interface IBranchInfoProvider : IProvider
    {
        /// <summary>
        /// Возвращает информацию о ветви на основе ее полного имени
        /// </summary>
        /// <param name="branchName">Полное имя ветви</param>
        /// <returns></returns>
        BranchInfo GetBranchInfo(string branchName);

        /// <summary>
        /// Возвращает информацию о ветви на основе ее полного имени и тега
        /// </summary>
        /// <param name="branchName">Полное имя ветви</param>
        /// <param name="tag">Тег, присваиваемый этой ветви</param>
        /// <returns></returns>
        BranchInfo GetBranchInfo(string branchName, object tag);
    }
}
