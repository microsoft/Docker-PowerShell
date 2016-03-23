using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = File.OpenRead(args[0]);
            var file2 = File.Create(args[1]);
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

                var task = writer.AddEntryAsync(entry);
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
