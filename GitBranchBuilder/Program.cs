using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using GitBranchBuilder.Pipelines;

namespace GitBranchBuilder
{
    class Program
    {
        static IContainer BuildAutofacContainer(Assembly assembly)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => !x.IsInterface)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder.Build();
        }
              
        static async Task Main(string[] args)
        {
            using var container = BuildAutofacContainer(Assembly.GetExecutingAssembly());
            var pipelines = container.Resolve<IEnumerable<IPipeline>>();
            
            await RunPipelines();

            Console.WriteLine("All job pipelines are ran to the end!");
            Console.ReadKey();
            
            // запуск всех конвейеров задач в асинхронном режиме
            Task RunPipelines()
            {
                var tasks =  pipelines
                    .Select(provider => (provider, options: new PipelineOptions { Container = container }))
                    .Select((data, index) =>
                    {
                        var provider = data.provider;
                        var task = Task.Run(() => provider.Run(data.options));

                        ContinueWithLogs(task, index + 1, DateTime.Now);

                        // освобождение ресурсов
                        return task.ContinueWith(x => provider.Dispose());
                    });

                return Task.WhenAll(tasks);
            }

            // вывод сообщений о состоянии выполнения конвейера
            static void ContinueWithLogs(Task task, int pipleineNumber, DateTime startTime)
            {
                task.ContinueWith(x => Console.WriteLine(
                    value: $"Pipeline #{pipleineNumber} finished successfully in {DateTime.Now - startTime}"),
                    continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);

                task.ContinueWith(x => Console.WriteLine(
                    value: $"Unable to finish pipeline #{pipleineNumber} because of exception: {x.Exception}"),
                    continuationOptions: TaskContinuationOptions.OnlyOnFaulted);

                task.ContinueWith(x => Console.WriteLine(
                    $"Execution of pipeline #{pipleineNumber} was cancelled"),
                    continuationOptions: TaskContinuationOptions.OnlyOnCanceled);
            }
        }
    }
}
