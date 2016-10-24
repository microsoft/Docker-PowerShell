using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.Generic;
using System.IO;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.Export, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [Alias("Save-ContainerImage")]
    public class ExportContainerImage : ImageOperationCmdlet
    {
        #region Parameters

        [Parameter(Position = 1,
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string DestinationFilePath { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            var filePath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, DestinationFilePath);
            var names = new List<string>(ParameterResolvers.GetImageIds(Image, ImageIdOrName));

            using (var fs = File.Create(filePath))
            using (var stream = await DkrClient.Miscellaneous.GetImagesAsTarballAsync(names.ToArray(), CmdletCancellationToken))
            using (CmdletCancellationToken.Register(() => stream.Dispose()))
            {
                await stream.CopyToAsync(fs);
            }
        }

        #endregion
    }
}