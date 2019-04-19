using Autofac;
using GitBranchBuilder.Jobs;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        static async Task<string> ExecuteJobAsync(IJob job)
        {
            void executeJob()
            {
                Console.WriteLine();
                job.Prepare();
                Console.WriteLine($"{job.Description}");
                job.Process();
            }

            try
            {
                if (job.IsThreadsafe)
                {
                    await Task.Run(executeJob);
                }
                else
                {
                    executeJob();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to finish the job because of exception: {ex}");
            }
            finally
            {
                job.Dispose();
            }

            return "OK";
        }

        static IEnumerable<string> RunProvidersAsync(IEnumerable<IJobPipeline> jobProviders)
        {
            foreach (var provider in jobProviders)
            {
                foreach (var job in provider.Jobs)
                {
                    yield return ExecuteJobAsync(job).Result;
                }
            }
        }

        static void Main(string[] args)
        {
            using var container = BuildAutofacContainer();

            var jobProviders = container.Resolve<IEnumerable<IJobPipeline>>();
            var jobs = jobProviders.SelectMany(x => x.Jobs);

            var results = RunProvidersAsync(jobProviders).ToList();
  
            Console.WriteLine("All jobs are finished!");
            Console.ReadKey();
        }
    }
}
