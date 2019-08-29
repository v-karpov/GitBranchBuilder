using CSharpFunctionalExtensions;

using GitBranchBuilder.Components;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Абстрактный класс работы, которая может быть выполнена несколько раз в случае неудачи
    /// </summary>
    /// <typeparam name="TInput">Тип входных данных</typeparam>
    /// <typeparam name="TResult">Тип результата</typeparam>
    public abstract class RetryJobBase<TJob, TInput, TResult> : PropagationJob<TInput, TResult>
        where TJob : ITrialJob<TInput, TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Работа, обернутая данным классом
        /// </summary>
        public TJob WrappedJob { get; set; }

        /// <summary>
        /// Сообщение, выводимое пользователю при попытке повторного выполнения
        /// </summary>
        public virtual string QuestionMessage { get; }

        /// <summary>
        /// Количество попыток перезапуска задачи при неудачном ее выполнении
        /// </summary>
        protected virtual int? RetryCount => default;

        /// <summary>
        /// Провайдер пользовательского подтверждения при получении ошибки
        /// </summary>
        public IUserApprovalService UserRetryApproval { get; set; }

        /// <summary>
        /// Внутренний шаг выполнения работы
        /// </summary>
        /// <param name="input">Входные данные для выполнения шага</param>
        /// <returns></returns>
        internal override TResult ExecuteInternal(TInput input)
            => WrappedJob.Execute(input);

        /// <summary>
        /// Уведомляет пользователя об очередной ошибке исполнения
        /// </summary>
        /// <param name="result">Ошибочный результат работы</param>
        protected virtual void FailureNotify(TResult result)
            => Log.Error($"Unable to proceed step {WrappedJob} because of failure");
           
        /// <summary>
        /// Производит выполнение работы в соответствии с ее параметрами
        /// </summary>
        /// <param name="input">Входные данные для подготовительного этапа</param>
        /// <returns></returns>
        public override TResult Execute(TInput input)
        {
            var result = ExecuteInternal(input);
            var retryStepsLeft = RetryCount;

            while (!result.IsSuccess && GetRetryApproval())
            {
                FailureNotify(result);
                result = ExecuteInternal(input);
            }

            ProcessAutocompletion(result);

            return result;

            bool GetRetryApproval() => 
                (retryStepsLeft.HasValue && retryStepsLeft-- > 0) || 
                UserRetryApproval.RequstApprove(QuestionMessage).IsSuccess;
        }
    }
}
