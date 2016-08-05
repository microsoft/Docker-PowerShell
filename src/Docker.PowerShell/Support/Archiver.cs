using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tar;

namespace Docker.PowerShell.Support
{
    internal static class Archiver
    {
        public static Stream CreateTarStream(IList<string> paths, CancellationToken cancellationToken, IProgress<string> progress = null)
        {
            var pipe = new Pipe();
            var reader = new PipeReadStream(pipe);
            try
            {
                var tarTask = Task.Run(async () =>
                {
                    using (var writer = new PipeWriteStream(pipe))
                    {
                        try
                        {
                            var tar = new TarWriter(writer);
                            foreach (var path in paths)
                            {
                                var fi = new FileInfo(path);
                                if (fi.Attributes.HasFlag(FileAttributes.Directory))
                                {
                                    await tar.CreateEntriesFromDirectoryAsync(path, ".", cancellationToken, progress);
                                }
                                else
                                {
                                    if (progress != null)
                                    {
                                        progress.Report(path);
                                    }

                                    await tar.CreateEntryFromFileAsync(path, Path.GetFileName(path), cancellationToken);
                                }
                            }
                            await tar.CloseAsync();
                        }
                        catch (Exception e)
                        {
                            writer.Close(e);
                            throw;
                        }
                    }
                }, cancellationToken);
            }
            catch (Exception e)
            {
                reader.Close(e);
                throw;
            }

            return reader;
        }
    }
}