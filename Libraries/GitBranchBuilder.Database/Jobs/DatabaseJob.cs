using System;
using System.Collections.Generic;
using System.Text;
using GitBranchBuilder.Components.Holders;
using Microsoft.SqlServer.Management.Smo;

namespace GitBranchBuilder.Jobs
{
    public abstract class DatabaseJob : TrialJob<Database, ResultWrapper<Database>> 
    {
        public Holder<Server> Server { get; set; }

        public Holder<Database> Database { get; set; }
    }
}
