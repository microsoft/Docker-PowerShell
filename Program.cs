using System;
using System.IO;
using Tar;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                using (var file = File.OpenRead(args[0]))
                {
                    var reader = new TarReader(file);
                    reader.ExtractDirectory(".").Wait();
                }
            }
            else if (args.Length > 2)
            {
                using (var file = File.Create(args[1]))
                {
                    string path = args[2];
                    var writer = new TarWriter(file);
                    writer.CreateEntriesFromDirectoryAsync(path, path).Wait();
                    writer.CloseAsync().Wait();
                }
            }
            else
            {
                using (var file = File.OpenRead(args[0]))
                using (var file2 = File.Create(args[1]))
                {
                    var reader = new TarReader(file);
                    var writer = new TarWriter(file2);
                    for (;;)
                    {
                        var entryTask = reader.GetNextEntryAsync();
                        entryTask.Wait();
                        var entry = entryTask.Result;
                        if (entry == null)
                        {
                            break;
                        }

                        var task = writer.CreateEntryAsync(entry);
                        try
                        {
                            task.Wait();
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        var buffer = new byte[entry.Length];
                        if (reader.CurrentFile.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            throw new Exception("what");
                        }

                        writer.CurrentFile.Write(buffer, 0, buffer.Length);
                        reader.CurrentFile.CopyTo(writer.CurrentFile);
                    }

                    writer.CloseAsync().Wait();
                }
            }
        }
    }
}
