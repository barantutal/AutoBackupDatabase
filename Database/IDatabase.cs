namespace AutoBackupDatabase.Database;

public interface IDatabase
{
    string GenerateBackupFile(string rootPath);
}