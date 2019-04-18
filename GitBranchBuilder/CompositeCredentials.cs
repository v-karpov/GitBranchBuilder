using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchBuilder
{
    public class CompositeCredentials
    {
        public Credentials BaseCredentials { get; }

        public string ApiToken { get; }

        public Identity Identity { get; }

        public Signature Signature
            => new Signature(Identity, DateTimeOffset.Now);

        public CompositeCredentials(Credentials baseCredentials, Identity identity, string apiToken)
        {
            BaseCredentials = baseCredentials;
            ApiToken = apiToken;
            Identity = identity;
        }

        public CompositeCredentials(string user, string password, string userName, string email,  string apiToken)
            : this(baseCredentials: new UsernamePasswordCredentials { Username = user, Password = password },
                   identity: new Identity(userName, email),
                   apiToken: apiToken)
        {

        }
    }
}
