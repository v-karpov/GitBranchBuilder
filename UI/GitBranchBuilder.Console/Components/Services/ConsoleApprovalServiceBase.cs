using System;

using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Components.Services
{
    /// <summary>
    /// Базовый класс сервиса подтверждения пользователем через консоль
    /// </summary>
    public abstract class ConsoleApprovalServiceBase : IUserApprovalService
    {
        /// <summary>
        /// Набор клавиш, по которым происходит подтверждение
        /// </summary>
        public virtual IKeySet Approval { get; } = SpecificKeySet.ApproveDefault;

        /// <summary>
        /// Набор клавиш, при нажатии которых происходит отказ
        /// </summary>
        public virtual IKeySet Refuse { get; } = SpecificKeySet.RefuseDefault;

        /// <summary>
        /// Сообщение при отказе
        /// </summary>
        public virtual string RefusalMessage { get; } = string.Empty;

        /// <summary>
        /// Показыват сообщение пользователю и возвращает <see cref="Result.Ok"/> в случае подтверждения
        /// </summary>
        /// <param name="message">Сообщение, содерадщее просьбу и описание вариантов ответа для пользователя</param>
        /// <returns></returns>
        public IResult RequstApprove(string message)
        {
            bool hasApprove = false, hasRefuse = false;
            ConsoleKeyInfo current;

            while (!(hasApprove || hasRefuse))
            {
                Console.WriteLine(message, $"Press {Approval.Description} to");
                current = Console.ReadKey(intercept: true);

                hasApprove = Approval.Contains(current.Key);
                hasRefuse = Refuse.Contains(current.Key);
            }

            return hasApprove ? Result.Ok() : Result.Fail(RefusalMessage);
        }
    }
}
