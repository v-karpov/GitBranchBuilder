using System;
using System.IO;

using GitBranchBuilder.Jobs;
using GitBranchBuilder.Pipelines.Merge.Data;

using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace GitBranchBuilder.Pipelines.Merge
{
    public class BuildJob : PropagationJob<string, BuildJobResult>, IMergeJob
    {
        public override string Description => $"Building contents of the branch";

        public string BuildPath { get; }

        public Project Project { get; protected set; }

        public LoggerVerbosity Verbosity { get; protected set; }

        public ILogger Logger { get; protected set; }

        public BuildJob(
            IRepositoryProvider repositoryProvider,
            IConfigurationProvider configurationProvider)
        {
            var repo = repositoryProvider.Repository;
            var buildConfig = configurationProvider.Configuration["Build"];
            var repoConfig = configurationProvider.Configuration["Repository"];

            BuildPath = buildConfig["Path"].StringValue;
            Verbosity = Enum.TryParse(buildConfig["Verbosity"].StringValue, out LoggerVerbosity verbosity)
                    ? verbosity
                    : LoggerVerbosity.Minimal;

            Prepare = input =>
            {
                Logger = new ConsoleLogger(Verbosity);

                Project = Project.FromFile(
                    file: Path.Combine(repoConfig["Path"].StringValue, BuildPath), 
                    options: new ProjectOptions
                    {
                        LoadSettings = (ProjectLoadSettings)buildConfig["Settings"].IntValue
                    });
            };

            Process = () =>
            {
                var result = new BuildJobResult() { IsSuccessful = true };

                while (!Project.Build(Logger))
                {
                    Console.WriteLine();
                    Console.WriteLine("Unable to build project. Fix the errors please and press any key to continue. \r\n Press ESC to halt pipeline.");

                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        result.IsSuccessful = false;
                        break;
                    }
                }

                return result;
            };
        }
    }
}
