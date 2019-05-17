using System;
using CSharpFunctionalExtensions;

namespace GitBranchBuilder.Components
{
    /// <summary>
    /// Сервис, позволяющий получить подтверждение пользователя
    /// </summary>
    public interface IUserApprovalService : IComponent
    {
        /// <summary>
        /// Показыват сообщение пользователю и возвращает <see cref="Result.Ok"/> в случае подтверждения
        /// </summary>
        /// <param name="message">Сообщение, содерадщее просьбу и описание вариантов ответа для пользователя</param>
        /// <returns></returns>
        Result RequstApprove(string message);
    }

    /// <summary>
    /// Сервис, позволяющий получить подтверждение пользователя через консоль
    /// </summary>
    public class ConsoleUserApprovalService : IUserApprovalService
    {
        /// <summary>
        /// Сообщение о том, что пользователь отказал в выполнении
        /// </summary>
        public const string UserDeniedMessage = "User refused to approve retrial";

        /// <summary>
        /// Показыват сообщение пользователю и возвращает <see cref="Result.Ok"/> в случае подтверждения
        /// </summary>
        /// <param name="message">Сообщение, содерадщее просьбу и описание вариантов ответа для пользователя</param>
        /// <returns></returns>
        public Result RequstApprove(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);

            if (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                return Result.Ok();
            }
            else
            {
                return Result.Fail(UserDeniedMessage);
            }
        }
    }
}