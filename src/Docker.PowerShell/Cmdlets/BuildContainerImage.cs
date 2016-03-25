using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;
using System.IO.Pipes;
using Docker.DotNet.Models;
using Tar;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Build", "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(Image))]
    public class BuildContainerImage : DkrCmdlet
    {
        private string _successfullyBuilt = "Successfully built ";

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
        public SwitchParameter SkipCache { get; set; }

        [Parameter]
        public SwitchParameter ForceRemoveIntermediateContainers { get; set; }

        [Parameter]
        public SwitchParameter PreserveIntermediateContainers { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            using (var reader = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.None, 65536))
            {
                var tarTask = Task.Run(async () =>
                {
                    using (var writer = new AnonymousPipeClientStream(PipeDirection.Out, reader.ClientSafePipeHandle))
                    {
                        var tar = new TarWriter(writer);
                        await tar.CreateEntriesFromDirectoryAsync(string.IsNullOrEmpty(Path) ? "." : Path, ".");
                        await tar.CloseAsync();
                        writer.Close();
                    }
                });

                var parameters = new BuildImageFromDockerfileParameters
                {
                    NoCache = SkipCache.ToBool(),
                    ForceRemoveIntermediateContainers = ForceRemoveIntermediateContainers.ToBool(),
                    RemoveIntermediateContainers  = !PreserveIntermediateContainers.ToBool(),
                };

                string repoTag = null;
                if (!string.IsNullOrEmpty(Repository))
                {
                    repoTag = Repository;
                    if (!string.IsNullOrEmpty(Tag))
                    {
                        repoTag += ":";
                        repoTag += Tag;
                    }
                    parameters.RepositoryTagName = repoTag;
                }
                else if (!string.IsNullOrEmpty(Tag))
                {
                    throw new Exception("tag without a repo???");
                }

                string imageId = null;
                bool failed = false;

                var buildTask = DkrClient.Miscellaneous.BuildImageFromDockerfileAsync(reader, parameters, CancelSignal.Token);
                using (var progress = buildTask.AwaitResult())
                using (var progressReader = new StreamReader(progress, new UTF8Encoding(false)))
                {
                    string line;
                    while ((line = progressReader.ReadLine()) != null)
                    {
                        var message = JsonConvert.DeserializeObject<JsonMessage>(line);
                        if (message.Stream != null)
                        {
                            if (message.Stream.StartsWith(_successfullyBuilt))
                            {
                                // This is probably the image ID.
                                imageId = message.Stream.Substring(_successfullyBuilt.Length).Trim();
                            }

                            var infoRecord = new HostInformationMessage();
                            infoRecord.Message = message.Stream;
                            WriteInformation(infoRecord, new string[] { "PSHOST" });
                        }

                        if (message.Error != null)
                        {
                            var error = new ErrorRecord(new Exception(message.Error.Message), null, ErrorCategory.OperationStopped, null);
                            WriteError(error);
                            failed = true;
                        }
                    }
                }

                tarTask.WaitUnwrap();
                if (imageId == null && !failed)
                {
                    throw new Exception("Could not find image, but no error was returned");
                }

                WriteObject(ContainerOperations.GetImageById(imageId, DkrClient));
            }
        }

        #endregion
    }
}
