using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace GitBranchBuilder.Jobs
{
    public class BackupJob : DatabaseJob
    {
        public BackupJob()
        {
            Prepare = database =>
            {

            };

            TryProcess = () =>
            {
                var backup = new Backup();
                var database = Database.Value;

                backup.PercentComplete += Backup_PercentComplete;
                backup.Complete += Backup_Complete;

                try
                {
                    backup.SqlBackup(Server);
                    return ResultWrapper.Ok(database);
                }
                catch (FailedOperationException smoException)
                {
                    return ResultWrapper.Fail(database, smoException.Message);
                }
                finally
                {
                    backup.Complete -= Backup_Complete;
                    backup.PercentComplete -= Backup_PercentComplete;
                }
            };
        }

        private void Backup_Complete(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
