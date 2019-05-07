using System.IO;

using GitBranchBuilder.Components;

namespace GitBranchBuilder.Providers.Build
{
    public class BuildPathProvider : Provider<BuildPath>
    {
        public BuildPathProvider(ConfigurationHolder config)
        {
            ValueGetter = () => new BuildPath
            {
                FullPath = Path.Combine(config["Repository", "Path"], config["Build", "Path"])
            };
        }
    }
}
