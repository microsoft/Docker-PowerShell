using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Docker.PowerShell.Support;
using Tar;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Copy, "ContainerFile",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class CopyContainerFile : SingleContainerOperationCmdlet
    {
        [Parameter(Mandatory = true, Position = 1)]
        [ValidateNotNullOrEmptyAttribute]
        public string[] Path { get; set; }

        [Parameter]
        [ValidateNotNullOrEmptyAttribute]
        public string Destination { get; set; }

        [Parameter]
        public SwitchParameter ToContainer { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            if (Container != null)
            {
                ContainerIdOrName = Container.ID;
            }

            if (ToContainer)
            {
                var hostPaths = Path.SelectMany(path =>
                {
                    ProviderInfo provider;
                    var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath(path, out provider);
                    if (provider.Name != "FileSystem")
                    {
                        throw new Exception(string.Format("The path {0} is not in the file system.", path));
                    }

                    return resolvedPaths;
                }).ToList();

                var p = new ContainerPathStatParameters
                {
                    Path = Destination ?? "."
                };

                var progress = new Progress<string>();
                progress.ProgressChanged += (o, s) => WriteVerbose(string.Format("Sending {0}", s));

                using (var reader = Archiver.CreateTarStream(hostPaths, CmdletCancellationToken, progress))
                {
                    await DkrClient.Containers.ExtractArchiveToContainerAsync(ContainerIdOrName, p, reader, CmdletCancellationToken);
                }
            }
            else
            {
                var hostPath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Destination ?? "");
                foreach (var singlePath in Path)
                {
                    var p = new GetArchiveFromContainerParameters
                    {
                        Path = singlePath
                    };

                    var response = await DkrClient.Containers.GetArchiveFromContainerAsync(ContainerIdOrName, p, false, CmdletCancellationToken);
                    using (var stream = response.Stream)
                    {
                        var progress = new Progress<string>();
                        progress.ProgressChanged += (o, s) => WriteVerbose(string.Format("Extracting {0}", s));

                        var tarReader = new TarReader(stream);
                        await tarReader.ExtractDirectoryAsync(hostPath, CmdletCancellationToken, progress);
                    }
                }
            }
        }
    }
}