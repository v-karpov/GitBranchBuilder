using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitBranchBuilder.Repo
{
    public class DefaultRepositoryProvider : ConfigurationHolder, IRepositoryProvider
    {
        public Repository Repository => RepositoryLoader.Value;

        protected Lazy<Repository> RepositoryLoader { get; }

        protected string Path { get; }

        public DefaultRepositoryProvider(IConfigurationProvider configurationProvider)
            : base(configurationProvider)
        {
            Path = Configuration["Repository"]["Path"].StringValue;

            RepositoryLoader = new Lazy<Repository>(() => new Repository(Path));
        }
    }
}
