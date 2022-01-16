using AutoBackupDatabase.Backup;
using AutoBackupDatabase.Database;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace AutoBackupDatabase.DataProviers;

public class DropboxProvider : IDataProvider
{
    const string dropboxToken = "**YOUR DROPBOX APP TOKEN**";
    const string dropboxFolder = "/backups/";
    
    public string GenerateBackup(IBackup backup, IDatabase database)
    {
        return backup.GenerateBackupFile(database);
    }

    public async Task Upload(string fullPath)
    {
        using var client = new DropboxClient(dropboxToken);
        const int chunkSize = 50 * 1024 * 1024;

        var fileContent = await System.IO.File.ReadAllBytesAsync(fullPath);
        new Random().NextBytes(fileContent);

        await using var stream = new MemoryStream(fileContent);
        var numChunks = (int)Math.Ceiling((double)stream.Length / chunkSize);

        byte[] buffer = new byte[chunkSize];
        string sessionId = null;

        for (var idx = 0; idx < numChunks; idx++)
        {
            var byteRead = stream.Read(buffer, 0, chunkSize);

            await using MemoryStream memStream = new MemoryStream(buffer, 0, byteRead);
            if (idx == 0)
            {
                var result = await client.Files.UploadSessionStartAsync(body: memStream);
                sessionId = result.SessionId;
            }

            else
            {
                UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(chunkSize * idx));

                if (idx == numChunks - 1)
                {
                    await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(dropboxFolder + Path.GetFileName(fullPath)), memStream);
                }

                else
                {
                    await client.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                }
            }
        }
    }

    public async Task DeleteAsync(string fileNameForStorage)
    {
        using var client = new DropboxClient(dropboxToken);
        await client.Files.DeleteV2Async(dropboxFolder + fileNameForStorage).ConfigureAwait(false);
    }
}