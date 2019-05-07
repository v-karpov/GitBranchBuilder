using System;

using GitBranchBuilder.Jobs;
using GitBranchBuilder.Providers.Build;

using Microsoft.Build.Framework;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class BuildJob : PropagationJob<string, BuildEngineResult>, IMergeJob
    {
        public override string Description => $"Building contents of the branch";

        public BuildJob(IBuildProvider buildProvider)
        {
            Process = () =>
            {
                BuildEngineResult result = buildProvider.GetValue();

                while (!result.Result)
                {
                    Console.WriteLine();
                    Console.WriteLine("Unable to build project. Fix the errors please and press any key to continue. \r\n Press ESC to halt pipeline.");

                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    result = buildProvider.GetValue();
                }

                return result;
            };
        }
    }
}
