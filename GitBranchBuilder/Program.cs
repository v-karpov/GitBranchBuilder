using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Autofac;

using GitBranchBuilder.Jobs;

namespace GitBranchBuilder
{
    class Program
    {
        static IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
                .Where(x => !x.IsInterface)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder.Build();
        }

        static async Task RunProvidersAsync(IEnumerable<IJobPipeline> jobProviders)
        {
            foreach (var provider in jobProviders)
            {
                try
                {
                    await provider.ExecuteAction();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to finish the job because of exception: {ex}");
                }
                finally
                {
                    provider.Dispose();
                }
            }
        }

        static async Task Main(string[] args)
        {
            using var container = BuildAutofacContainer();

            var jobProviders = container.Resolve<IEnumerable<IJobPipeline>>();

            await RunProvidersAsync(jobProviders);
  
            Console.WriteLine("All jobs are finished!");
            Console.ReadKey();
        }
    }
}
