using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpConfig;

namespace GitBranchBuilder
{
    public abstract class ConfigurationHolder
    {
        protected virtual Configuration Configuration { get; }

        public ConfigurationHolder(IConfigurationProvider configurationProvider)
        {
            Configuration = configurationProvider.Configuration;
        }
    }
}
