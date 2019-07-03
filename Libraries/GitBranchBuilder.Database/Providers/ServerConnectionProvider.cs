using System;
using System.Data.SqlClient;
using GitBranchBuilder.Components;

using Microsoft.SqlServer.Management.Common;

namespace GitBranchBuilder.Providers
{
    public class ServerConnectionProvider : Provider<ServerConnection>
    {
        public MaybeHolder<SqlConnectionInfo> ConnectionInfo { get; set; }

        public MaybeHolder<SqlConnection> Connection { get; set; }

        public MaybeHolder<IRenewableToken> RenewableToken { get; set; }

        public override ServerConnection GetValue()
        {
            if (RenewableToken.HasValue)
            {
                return new ServerConnection(RenewableToken.Value);
            }
            else if (Connection.HasValue)
            {
                return new ServerConnection(Connection);
            }
            else if (ConnectionInfo.HasValue)
            {
                return new ServerConnection(ConnectionInfo);
            }
            else
            {
#if DEBUG
                throw new NotImplementedException(nameof(ServerConnection));
#else
                return default;
#endif
            }
        }
    }
}
