using System;
using System.Collections.Generic;
using System.Text;
using GitBranchBuilder.Jobs;
using GitBranchBuilder.Jobs.Combined;
using GitBranchBuilder.Pipelines.Configarable;

namespace GitBranchBuilder
{
    public interface IUpdatePieplineJob : IJob
    {

    }

    public class UpdatePipeline : ConfigurablePipeline<IUpdatePieplineJob>
    {
        class PipelineConfigurator : PipelineConfigurator<IUpdatePieplineJob>
        {
            public PipelineConfigurator()
            {

            }
        }
    }
}
