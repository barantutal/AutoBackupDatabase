using System.Text;
using AutoBackupDatabase.Console;

namespace AutoBackupDatabase.Database;

public class MySqlDatabase : IDatabase
{
    public string GenerateBackupFile(string rootPath)
    {
        var fileName = $"alldb{DateTime.Now:dd-MM-yyyy-HH-mm}.sql.gz";
        var fulPath = Path.Combine(rootPath, fileName);

        var command = new StringBuilder();
        command.Append("mysqldump -u root_ ");
        command.Append("--routines ");
        command.Append("--default-character-set=utf8 ");
        command.Append("--all-databases ");
        command.Append("| gzip > ");
        command.Append(fulPath);

        Bin.Bash(command.ToString());

        return fulPath;
    }
}