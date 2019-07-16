using System;
using System.Collections.Generic;
using System.Text;
using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Components.Holders.Specific;
using Microsoft.SqlServer.Management.Smo;

namespace GitBranchBuilder.Providers
{
    public class DatabaseProvider : Provider<Database>
    {
        public Holder<Server> Server { get; set; }

        public ConfigurationHolder Configuration { get; set; }

        public override Database GetValue()
            => Server.Value.Databases[Configuration["SQL", "DatabaseName"]];
    }
}
