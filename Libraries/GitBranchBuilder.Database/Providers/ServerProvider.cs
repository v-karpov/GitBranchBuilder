using GitBranchBuilder.Components.Holders;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace GitBranchBuilder.Providers
{
    public class ServerProvider : Provider<Server>
    {
        public Holder<ServerConnection> Connection { get; set; }

        public override Server GetValue()
            => new Server(Connection);
    }
}
