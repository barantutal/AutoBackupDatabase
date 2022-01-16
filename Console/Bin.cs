using System.Diagnostics;

namespace AutoBackupDatabase.Console;

public static class Bin
{
    public static string Bash(string cmd)
    {
        string str = cmd.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + str + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        string end = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return end;
    }
}