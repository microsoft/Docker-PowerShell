using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public static class TarWriterExtensions
    {
        private static async Task CreateEntryFromFileInfoAsync(this TarWriter writer, FileInfo fi, string entryName, CancellationToken cancellationToken)
        {
            var entry = new TarEntry
            {
                Name = entryName.Replace('\\', '/').TrimEnd('/'),
                AccessTime = fi.LastAccessTimeUtc,
                ModifiedTime = fi.LastWriteTimeUtc,
                Type = TarEntryType.File,
                Mode = Convert.ToInt32("644", 8),
            };

            if (fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                entry.Type = TarEntryType.Directory;
                entry.Mode = Convert.ToInt32("755", 8);
                entry.Name += "/";
            }
            else
            {
                entry.Length = fi.Length;
            }

            if (fi.Attributes.HasFlag(FileAttributes.ReadOnly))
            {
                entry.Mode &= ~Convert.ToInt32("222", 8);
            }

            await writer.CreateEntryAsync(entry);

            if (!fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                using (var file = fi.OpenRead())
                {
                    const int bufferSize = 81920; // This is the documented default for CopyToAsync().
                    await file.CopyToAsync(writer.CurrentFile, bufferSize, cancellationToken);
                }
            }
        }

        public static Task CreateEntryFromFileAsync(this TarWriter writer, string path, string entryName, CancellationToken cancellationToken)
        {
            var fi = new FileInfo(path);
            return writer.CreateEntryFromFileInfoAsync(fi, entryName, cancellationToken);
        }

        public static async Task CreateEntriesFromDirectoryAsync(this TarWriter writer, string path, string entryBase, CancellationToken cancellationToken, IProgress<string> progress = null)
        {
            if (progress != null)
            {
                progress.Report(path);
            }

            await CreateEntryFromFileAsync(writer, path, entryBase, cancellationToken);

            // Keep a stack of directory enumerations in order to write the
            // tar in depth-first order (which seems to be more common in tar
            // implementations than the breadth-first order that SearchOption.AllDirectories
            // implements).
            var stack = new List<IEnumerator<string>>();
            IEnumerator<string> enumerator = null;
            try
            {
                enumerator = Directory.EnumerateFileSystemEntries(path).GetEnumerator();
                while (enumerator != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    while (enumerator.MoveNext())
                    {
                        var filePath = enumerator.Current;
                        var fileName = filePath.Substring(path.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        var fi = new FileInfo(filePath);
                        var tarPath = Path.Combine(entryBase, fileName);
                        if (progress != null)
                        {
                            progress.Report(filePath);
                        }

                        await writer.CreateEntryFromFileInfoAsync(fi, Path.Combine(entryBase, fileName), cancellationToken);
                        if (fi.Attributes.HasFlag(FileAttributes.Directory))
                        {
                            stack.Add(enumerator);
                            enumerator = Directory.EnumerateFileSystemEntries(filePath).GetEnumerator();
                        }
                    }

                    enumerator.Dispose();
                    enumerator = null;
                    if (stack.Count > 0)
                    {
                        enumerator = stack.Last();
                        stack.RemoveAt(stack.Count - 1);
                    }
                }
            }
            finally
            {
                enumerator?.Dispose();

                foreach (var e in stack)
                {
                    e.Dispose();
                }
            }
        }
    }
}
