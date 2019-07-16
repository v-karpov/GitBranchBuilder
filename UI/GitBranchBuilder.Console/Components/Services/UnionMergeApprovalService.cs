using System;

using GitBranchBuilder.Pipelines.Merge;

namespace GitBranchBuilder.Components.Services
{
    /// <summary>
    /// Сервис, позволяющий получить подтверждение пользователя через консоль
    /// </summary>
    public class UnionMergeApprovalService : ConsoleApprovalServiceBase, IMergeApprovalService
    {
        public override IKeySet Approval { get; } = new SpecificKeySet(ConsoleKey.U);
    }
}