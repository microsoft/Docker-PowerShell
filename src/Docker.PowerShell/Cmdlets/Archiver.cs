using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tar;

namespace Docker.PowerShell.Cmdlets
{
    internal static class Archiver
    {
        public static Stream CreateTarStream(string path, CancellationToken cancellationToken)
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
                            await tar.CreateEntriesFromDirectoryAsync(path, ".", cancellationToken);
                        }
                        catch (Exception e)
                        {
                            writer.Close(e);
                            throw;
                        }

                        writer.Close();
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