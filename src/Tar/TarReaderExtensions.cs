using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public static class TarReaderExtensions
    {
        public static async Task ExtractDirectoryAsync(this TarReader reader, string basePath, CancellationToken cancellationToken, IProgress<string> progress = null)
        {
            for (; ;)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var entry = await reader.GetNextEntryAsync();
                if (entry == null)
                {
                    break;
                }

                var path = Path.Combine(basePath, entry.Name);
                if (progress != null)
                {
                    progress.Report(entry.Name);
                }

                switch (entry.Type)
                {
                    default: // Don't know how to handle these. Pretend they are files.
                    case TarEntryType.File:
                        using (var file = File.OpenWrite(path))
                        {
                            const int bufferSize = 81920; // Default buffer size for CopyToAsync.
                            await reader.CurrentFile.CopyToAsync(file, bufferSize, cancellationToken);
                        }
                        File.SetLastWriteTimeUtc(path, entry.ModifiedTime);
                        break;

                    case TarEntryType.Directory:
                        Directory.CreateDirectory(path);
                        Directory.SetLastWriteTimeUtc(path, entry.ModifiedTime);
                        break;
                }

            }
        }
    }
}