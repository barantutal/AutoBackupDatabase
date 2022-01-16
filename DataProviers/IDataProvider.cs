using AutoBackupDatabase.Backup;
using AutoBackupDatabase.Database;

namespace AutoBackupDatabase.DataProviers;

public interface IDataProvider
{
    string GenerateBackup(IBackup backupType, IDatabase database);
    Task Upload(string fullPath);
}