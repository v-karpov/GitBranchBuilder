using GitBranchBuilder.Components;

using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

namespace GitBranchBuilder.Providers.Build
{
    public class ProjectOptionsProvider : Provider<ProjectOptions>
    {
        public ProjectOptionsProvider(ConfigurationHolder config)
        {
            ValueGetter = () => new ProjectOptions
            {
                LoadSettings = (ProjectLoadSettings)config.Int("Build", "Settings")
            };
        }
    }
}
