using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Docker.PowerShell.Objects;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Request, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ImagesListResponse))]
    [Alias("Pull-ContainerImage")]
    public class RequestContainerImage : DkrCmdlet
    {
        private const string StatusUpToDate = "Status: Image is up to date for ";
        private const string StatusDownloadedNewer = "Status: Downloaded newer image for ";

        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Repository { get; set; }

        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            Position = 1)]
        [ValidateNotNullOrEmpty]
        public string Tag { get; set; }

        [Parameter]
        public SwitchParameter All { get; set; }

        [Parameter]
        public AuthConfig Authorization { get; set; }

        #region Overrides
        protected override async Task ProcessRecordAsync()
        {
            string repoTag = null;
            bool failed = false;
            var pullTask = DkrClient.Images.PullImageAsync(new ImagesPullParameters() { All = All, Parent = Repository, Tag = Tag ?? "latest", }, Authorization);
            var messageWriter = new JsonMessageWriter(this);

            using (var pullStream = await pullTask)
            {
                // ReadLineAsync is not cancellable without closing the whole stream, so register a callback to do just that.
                using (CmdletCancellationToken.Register(() => pullStream.Dispose()))
                using (var pullReader = new StreamReader(pullStream, new UTF8Encoding(false)))
                {
                    string line;
                    while ((line = await pullReader.ReadLineAsync()) != null)
                    {
                        var message = JsonConvert.DeserializeObject<JsonMessage>(line);
                        if (message.Status != null)
                        {
                            if (message.Status.StartsWith(StatusUpToDate))
                            {
                                // This is probably the image repository:tag.
                                repoTag = message.Status.Substring(StatusUpToDate.Length).Trim();
                            }
                            else if (message.Status.StartsWith(StatusDownloadedNewer))
                            {
                                // This is probably the image repository:tag.
                                repoTag = message.Status.Substring(StatusDownloadedNewer.Length).Trim();
                            }
                        }

                        if (message.Error != null)
                        {
                            failed = true;
                        }

                        messageWriter.WriteJsonMessage(message);
                    }
                }
            }

            messageWriter.ClearProgress();
            if (repoTag != null)
            {
                WriteObject((await ContainerOperations.GetImagesByRepoTag(repoTag, DkrClient)).Single());
            }
            else if (!failed)
            {
                throw new Exception("Could not find image, but no error was returned");
            }
        }

        #endregion
    }
}
