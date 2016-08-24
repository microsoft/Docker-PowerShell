using System;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet.Models;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Import, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [Alias("Load-ContainerImage")]
    [OutputType(typeof(ImagesListResponse))]
    public class ImportContainerImage : DkrCmdlet
    {
        private const string LoadedImage = "Loaded image: ";

        #region Parameters

        [Parameter(Position = 0,
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string[] FilePath { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var item in FilePath)
            {
                var filePath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, item);

                string imageId = null;
                bool failed = false;

                using (var file = File.OpenRead(filePath))
                {
                    var loadTask = DkrClient.Miscellaneous.LoadImageFromTarball(file, new ImageLoadParameters { Quiet = false }, CmdletCancellationToken);

                    var messageWriter = new JsonMessageWriter(this);

                    using (var pushStream = await loadTask)
                    {
                        // ReadLineAsync is not cancellable without closing the whole stream, so register a callback to do just that.
                        using (CmdletCancellationToken.Register(() => pushStream.Dispose()))
                        using (var pullReader = new StreamReader(pushStream, new UTF8Encoding(false)))
                        {
                            string line;
                            while ((line = await pullReader.ReadLineAsync()) != null)
                            {
                                var message = JsonConvert.DeserializeObject<JsonMessage>(line);
                                if (message.Stream != null && message.Stream.StartsWith(LoadedImage))
                                {
                                    // This is probably the image ID.
                                    imageId = message.Stream.Substring(LoadedImage.Length).Trim();
                                }

                                if (message.Error != null)
                                {
                                    failed = true;
                                }

                                messageWriter.WriteJsonMessage(JsonConvert.DeserializeObject<JsonMessage>(line));
                            }
                        }
                    }

                    messageWriter.ClearProgress();
                    if (imageId != null)
                    {
                        WriteObject((await ContainerOperations.GetImagesByRepoTag(imageId, DkrClient)).Single());
                    }
                    else if (!failed)
                    {
                        throw new Exception("Could not find image, but no error was returned");
                    }
                }
            }
        }

        #endregion
    }
}