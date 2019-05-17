using System;

using LibGit2Sharp;

namespace GitBranchBuilder.Providers
{
    public class FetchOptionsProvdier : Provider<FetchOptions>
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public FetchOptionsProvdier(ICredentialsProvider credentialsProvider)
            => ValueGetter = () => new FetchOptions
            {
                CredentialsProvider = credentialsProvider.GetValue,
                // TODO: использовать NLog
                OnProgress = str => { Console.WriteLine(str); return true; }
            };
    }
}
