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
    [Cmdlet(VerbsLifecycle.Submit, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    [Alias("Push-ContainerImage")]
    public class SubmitContainerImage : DkrCmdlet
    {
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ImageArgumentCompleter))]
        [Alias("ImageName", "ImageId")]
        public string ImageIdOrName { get; set; }

        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ImagesListResponse Image { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        [Parameter]
        public AuthConfig Authorization { get; set; }

        #region Overrides
        protected override async Task ProcessRecordAsync()
        {
            string repoTag = null;

            if (ImageIdOrName != null)
            {
                repoTag = ImageIdOrName;
            }
            else if (Image.RepoTags.Count != 1)
            {
                throw new Exception("Ambiguous repository and tag: Supplied image must have only one repository:tag.");
            }
            else
            {
                repoTag = Image.RepoTags[0];
            }

            var pushTask = DkrClient.Images.PushImageAsync(repoTag, new ImagePushParameters(), Authorization);
            var messageWriter = new JsonMessageWriter(this);

            using (var pushStream = await pushTask)
            {
                // ReadLineAsync is not cancellable without closing the whole stream, so register a callback to do just that.
                using (CmdletCancellationToken.Register(() => pushStream.Dispose()))
                using (var pullReader = new StreamReader(pushStream, new UTF8Encoding(false)))
                {
                    string line;
                    while ((line = await pullReader.ReadLineAsync()) != null)
                    {
                        messageWriter.WriteJsonMessage(JsonConvert.DeserializeObject<JsonMessage>(line));
                    }
                }
            }

            messageWriter.ClearProgress();
            if (PassThru)
            {
                WriteObject((await ContainerOperations.GetImagesByRepoTag(repoTag, DkrClient)).Single());
            }
        }

        #endregion
    }
}
