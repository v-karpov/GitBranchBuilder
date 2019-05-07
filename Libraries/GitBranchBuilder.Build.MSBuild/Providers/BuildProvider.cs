using GitBranchBuilder.Components;

using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace GitBranchBuilder.Providers.Build
{
    public class BuildProvider : Provider<BuildEngineResult>, IBuildProvider
    {
        public BuildEngineResult GetValue(Project project, ILogger logger) 
            => new BuildEngineResult(project.Build(logger), default);

        public BuildEngineResult GetValue(BuildPath path, ILogger logger)
            => GetValue(Project.FromFile(path.FullPath, new ProjectOptions()), logger);

        public BuildProvider(
            Holder<Project> project,
            Holder<ILogger> logger)
        {
            ValueGetter = () => GetValue(project, logger.Value);
        }
    }
}
