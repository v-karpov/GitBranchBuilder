using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpConfig;

namespace GitBranchBuilder
{
    public interface IConfigurationProvider
    {
        Configuration Configuration { get; }

        void Save();

        void Reload();
    }
}
