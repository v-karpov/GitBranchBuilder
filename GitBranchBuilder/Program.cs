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
        static IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetCallingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => !x.IsInterface)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder.Build();
        }
              
        static async Task Main(string[] args)
        {
            using var container = BuildAutofacContainer();
            var pipelines = container.Resolve<IEnumerable<IPipeline>>();
            
            await Task.WhenAll(RunPipelines());

            Console.WriteLine("All job pipelines are ran to the end!");
            Console.ReadKey();

            IEnumerable<Task> RunPipelines()
            {
                var tasks =  pipelines
                    .Select(provider => (provider, options: new PipelineOptions()))
                    .Select((data, index) =>
                    {
                        var startTime = DateTime.Now;
                        var number = index + 1;

                        data.provider.Prepare(container);

                        return Task.Run(() => data.provider.Run(data.options))
                            // вывод информационных сообщений
                            .ContinueWith(x => Console.WriteLine($"Pipeline #{number} finished successfully in {(DateTime.Now - startTime)}"), TaskContinuationOptions.OnlyOnRanToCompletion)
                            .ContinueWith(x => Console.WriteLine($"Unable to finish pipeline #{number} because of exception: {x.Exception}"), TaskContinuationOptions.OnlyOnFaulted)
                            .ContinueWith(x => Console.WriteLine($"Execution of pipeline #{number} was cancelled"), TaskContinuationOptions.OnlyOnCanceled)
                            //// освобождение ресурсов
                            .ContinueWith(x => data.provider.Dispose());
                    });

                return tasks;
            }
        }
    }
}
