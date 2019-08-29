using GitBranchBuilder.Jobs;
using GitBranchBuilder.Pipelines.Configarable;

namespace GitBranchBuilder
{
    public class UpdatePipeline : ConfigurablePipeline
    {
        class Config : PipelineConfigurator
        {
            public Config()
            {

            }
        }
    }
}
