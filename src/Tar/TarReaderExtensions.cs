using System.IO;
using System.Threading.Tasks;

namespace Tar
{
    public static class TarReaderExtensions
    {
        public static async Task ExtractDirectory(this TarReader reader, string basePath)
        {
            for (; ;)
            {
                var entry = await reader.GetNextEntryAsync();
                if (entry == null)
                {
                    break;
                }

                var path = Path.Combine(basePath, entry.Name);

                switch (entry.Type)
                {
                    default: // Don't know how to handle these. Pretend they are files.
                    case TarEntryType.File:
                        using (var file = File.OpenWrite(path))
                        {
                            await reader.CurrentFile.CopyToAsync(file);
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