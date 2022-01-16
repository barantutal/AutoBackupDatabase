using AutoBackupDatabase.Backup;
using AutoBackupDatabase.Database;
using AutoBackupDatabase.DataProviers;
using Coravel.Invocable;

namespace AutoBackupDatabase.Jobs;

public class MySqlBackupJob : IInvocable
{
    private const string _rootPath = "/var/backups/";
    public async Task Invoke()
    {
        IDataProvider dataProvider = new DropboxProvider();
        var fullPath = dataProvider.GenerateBackup(new FileBackup(), new MySqlDatabase());
        await dataProvider.Upload(fullPath);
    }
}