using System;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Docker.PowerShell.Support;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [Alias("Build-ContainerImage")]
    [OutputType(typeof(ImagesListResponse))]
    public class NewContainerImage : DkrCmdlet
    {
        private string SuccessfullyBuilt = "Successfully built ";

        #region Parameters

        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Repository { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Tag { get; set; }

        [Parameter]
        public IsolationType Isolation { get; set; }

        [Parameter]
        public SwitchParameter SkipCache { get; set; }

        [Parameter]
        public SwitchParameter ForceRemoveIntermediateContainers { get; set; }

        [Parameter]
        public SwitchParameter PreserveIntermediateContainers { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            var directory = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path ?? "");

            // Ensure the path is a directory.
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory);
            }

            WriteVerbose(string.Format("Archiving the contents of {0}", directory));

            using (var reader = Archiver.CreateTarStream(new List<string> { directory }, CmdletCancellationToken))
            {
                var parameters = new ImageBuildParameters
                {
                    NoCache = SkipCache,
                    ForceRemove = ForceRemoveIntermediateContainers,
                    Remove = !PreserveIntermediateContainers,
                };

                if (this.Isolation != IsolationType.Default)
                {
                    parameters.Isolation = this.Isolation.ToString();
                }

                string repoTag = null;
                if (!string.IsNullOrEmpty(Repository))
                {
                    repoTag = Repository;
                    if (!string.IsNullOrEmpty(Tag))
                    {
                        repoTag += ":";
                        repoTag += Tag;
                    }

                    parameters.Tags = new List<string>
                    {
                        repoTag
                    };
                }
                else if (!string.IsNullOrEmpty(Tag))
                {
                    throw new Exception("You must specify a repository name in order to specify a tag.");
                }

                string imageId = null;
                bool failed = false;

                var progress = new Progress<ProgressReader.Status>();
                var progressRecord = new ProgressRecord(0, "Dockerfile context", "Uploading");
                progress.ProgressChanged += (o, status) =>
                {
                    if (status.Complete)
                    {
                        progressRecord.CurrentOperation = null;
                        progressRecord.StatusDescription = "Processing";
                    }
                    else
                    {
                        progressRecord.StatusDescription = string.Format("Uploaded {0} bytes", status.TotalBytesRead);
                    }

                    WriteProgress(progressRecord);
                };

                var progressReader = new ProgressReader(reader, progress, 512 * 1024);
                var buildTask = DkrClient.Miscellaneous.BuildImageFromDockerfileAsync(progressReader, parameters, CmdletCancellationToken);
                var messageWriter = new JsonMessageWriter(this);

                using (var buildStream = await buildTask)
                {
                    // Complete the upload progress bar.
                    progressRecord.RecordType = ProgressRecordType.Completed;
                    WriteProgress(progressRecord);

                    // ReadLineAsync is not cancellable without closing the whole stream, so register a callback to do just that.
                    using (CmdletCancellationToken.Register(() => buildStream.Dispose()))
                    using (var buildReader = new StreamReader(buildStream, new UTF8Encoding(false)))
                    {
                        string line;
                        while ((line = await buildReader.ReadLineAsync()) != null)
                        {
                            var message = JsonConvert.DeserializeObject<JsonMessage>(line);
                            if (message.Stream != null && message.Stream.StartsWith(SuccessfullyBuilt))
                            {
                                // This is probably the image ID.
                                imageId = message.Stream.Substring(SuccessfullyBuilt.Length).Trim();
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
                if (imageId != null)
                {
                    WriteObject(await ContainerOperations.GetImageById(imageId, DkrClient));
                }
                else if (!failed)
                {
                    throw new Exception("Could not find image, but no error was returned");
                }
            }
        }

        #endregion
    }
}
