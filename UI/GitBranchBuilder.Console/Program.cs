using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Autofac;

using GitBranchBuilder.Components;
using GitBranchBuilder.Pipelines;
using GitBranchBuilder.Providers;

using MoreLinq;

namespace GitBranchBuilder
{
    class Program
    {
        static IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();

            void RegisterAssembly(Assembly assembly)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .Where(x => !(x.IsInterface || x.IsAbstract))
                    .AsSelf()
                    //.AsImplementedInterfaces()
                    .As(t => t.GetInterfaces().Where(i => !i.FullName.StartsWith("System")))
                    .SingleInstance()
                    .PropertiesAutowired();
            }

            var plugins = Directory
                .GetFiles($"{Environment.CurrentDirectory}", "GitBranchBuilder*.dll", SearchOption.TopDirectoryOnly)
                .Select(Assembly.LoadFile);

            var types = plugins
                .SelectMany(x => Autofac.Util.AssemblyExtensions.GetLoadableTypes(x));
                         
            builder.RegisterGeneric(typeof(Holder<>))
                .SingleInstance();

            builder.RegisterGeneric(typeof(DefaultProvider<>))
                .As(typeof(IProvider<>))
                .SingleInstance();
               
            plugins.ForEach(RegisterAssembly);

            return builder.Build();
        }
     
        static async Task Main(string[] args)
        {
            using var container = BuildAutofacContainer();
            var pipelines = container.Resolve<IEnumerable<IPipeline>>();
            
            await RunPipelines();

            Console.WriteLine("All job pipelines are ran to the end!");
            Console.ReadKey();
            
            // запуск всех конвейеров задач в асинхронном режиме
            Task RunPipelines()
            {
                var tasks =  pipelines
                    .Select(provider => (provider, options: new StartOptions()))
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
            static void ContinueWithLogs(Task task, int number, DateTime startTime)
            {
                var pipleineId = $"pipeline #{number}";

                task.ContinueWith(x => Console.WriteLine(
                    value: $"The {pipleineId} was finished successfully in {DateTime.Now - startTime}"),
                    continuationOptions: TaskContinuationOptions.OnlyOnRanToCompletion);

                task.ContinueWith(x => Console.WriteLine(
                    value: $"Unable to finish {pipleineId} because of exception: {x.Exception}"),
                    continuationOptions: TaskContinuationOptions.OnlyOnFaulted);

                task.ContinueWith(x => Console.WriteLine(
                    value: $"Execution of {pipleineId} was cancelled"),
                    continuationOptions: TaskContinuationOptions.OnlyOnCanceled);
            }
        }
    }
}
