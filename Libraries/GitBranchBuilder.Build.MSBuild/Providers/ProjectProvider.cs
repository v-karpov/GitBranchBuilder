using GitBranchBuilder.Components;

using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

namespace GitBranchBuilder.Providers.Build
{
    public class ProjectProvider : Provider<Project>
    {
        public ProjectProvider(
            Holder<BuildPath> buildPath,
            Holder<ProjectOptions> options)
        {
            ValueGetter = () => Project.FromFile(buildPath.Value.FullPath, options);
        }
    }
}
