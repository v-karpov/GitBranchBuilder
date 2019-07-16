using System;
using GitBranchBuilder.Components.Holders;
using LibGit2Sharp;
using NLog;

namespace GitBranchBuilder.Providers
{
    public class FetchOptionsProvdier : Provider<FetchOptions>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public FetchOptionsProvdier(ICredentialsProvider credentialsProvider, Holder<ILogger> logger)
            => ValueGetter = () => new FetchOptions
            {
                CredentialsProvider = credentialsProvider.GetValue,
                OnProgress = str => { logger.Value.Info(str); return true; }
            };
    }
}
