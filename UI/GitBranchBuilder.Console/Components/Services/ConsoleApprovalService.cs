using System;

using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Components.Services
{
    public interface IConsoleApprovalService : IUserApprovalService
    {

    }

    /// <summary>
    /// Сервиса подтверждения пользователем через консоль
    /// </summary>
    public class ConsoleApprovalService : ConsoleApprovalServiceBase, IConsoleApprovalService
    {

    }
}
