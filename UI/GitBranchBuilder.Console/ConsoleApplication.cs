using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Components.Services;
using GitBranchBuilder.Pipelines;

using MoreLinq;

using NLog;

namespace GitBranchBuilder
{
    /// <summary>
    /// Консольное приложение для запуска конвейеров
    /// </summary>
    public sealed class ConsoleApplication
    {
        /// <summary>
        /// Набор конвейеров для запуска
        /// </summary>
        public IEnumerable<IPipeline> Pipelines { get; }

        /// <summary>
        /// Сервис подтверждения пользовательских действий
        /// </summary>
        public IConsoleApprovalService UserApproval { get; }

        /// <summary>
        /// Логгер для использования в приложении
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Запускает приложение с указанным набором загруженных типов
        /// </summary>
        /// <param name="loadedTypes">Набор загруженных типов</param>
        public async Task Run(IEnumerable<string> loadedTypes)
        {
#if DEBUG
            Logger.Debug("IoC container loaded these types: \r\n");
            loadedTypes.OrderBy(x => x).ForEach(x => Logger.Trace(x));
#endif
            await RunPipelines();

            UserApproval.RequstApprove("All job pipelines are ran to the end! {0} quit.");

            // запуск всех конвейеров задач в асинхронном режиме
            async Task RunPipelines()
            {
                var tasks = Pipelines
                    .Select(provider => (provider, options: new StartOptions()))
                    .Select((data, index) =>
                    {
                        var provider = data.provider;
                        var task = Task.Run(() => provider.Run(data.options));

                        ContinueWithLogs(task, index + 1, DateTime.Now);

                        // освобождение ресурсов
                        return task.ContinueWith(x => provider.Dispose());
                    });

                await Task.WhenAll(tasks);
            }

            // вывод сообщений о состоянии выполнения конвейера
            void ContinueWithLogs(Task task, int number, DateTime startTime)
            {
                var pipleineId = $"pipeline #{number}";

                task.ContinueWith(x => Logger.Info(
                    value: $"The {pipleineId} was finished successfully in {DateTime.Now - startTime}"),
                    continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);

                task.ContinueWith(x => Logger.Fatal(
                    value: $"Unable to finish {pipleineId} because of exception: {x.Exception}"),
                    continuationOptions: TaskContinuationOptions.OnlyOnFaulted);

                task.ContinueWith(x => Logger.Error(
                    value: $"Execution of {pipleineId} was cancelled"),
                    continuationOptions: TaskContinuationOptions.OnlyOnCanceled);
            }
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ConsoleApplication(IEnumerable<IPipeline> pipelines,
                                  IConsoleApprovalService userApprovalService,
                                  Holder<ILogger> holder)
        {
            Pipelines = pipelines;
            UserApproval = userApprovalService;
            Logger = holder.Value;
        }
    }
}
