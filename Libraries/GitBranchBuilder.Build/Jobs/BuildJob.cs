using System;

using GitBranchBuilder.Providers.Build;

using Microsoft.Build.Framework;

namespace GitBranchBuilder.Jobs
{
    /// <summary>
    /// Построение проекта с возможностью повторения при неудаче
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public class RetryBuildJob<TInput> : RetryJob<TInput, BuildEngineResult>
    {
        /// <summary>
        /// Реализация единичной попытки построения проекта
        /// </summary>
        public class BuildJobImpl : TrialJob<TInput, BuildEngineResult>
        {
            /// <summary>
            /// Описание работы
            /// </summary>
            public override string Description => $"Building contents of the branch";

            /// <summary>
            /// Провайдер построения проекта
            /// </summary>
            public IBuildProvider BuildProvider { get; set; }

            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            /// <param name="prepareAction"></param>
            public BuildJobImpl(Action<TInput> prepareAction = default)
            {
                Prepare = prepareAction ?? Prepare;
                TryProcess = () =>
                {
                    var result = BuildProvider.GetValue();
                    return result.Result
                        ? result
                        : throw new InvalidOperationException();
                };
            }
        }

        /// <summary>
        /// Формулировка вопроса при необходимости повторения работы
        /// </summary>
        public override string QuestionMessage 
            => "Unable to build project. Fix the errors please and press any key to continue. \r\n Press ESC to halt pipeline.";

    }
}
