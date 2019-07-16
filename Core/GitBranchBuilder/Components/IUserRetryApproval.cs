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
        IResult RequstApprove(string message);
    }
}