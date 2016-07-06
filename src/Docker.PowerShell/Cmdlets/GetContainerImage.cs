using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.ImageName)]
    [OutputType(typeof(ImagesListResponse))]
    public class GetContainerImage : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// The specific image name to get.
        /// </summery>
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageName,
            ValueFromPipeline = true,
                   Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ImageArgumentCompleter))]
        public string[] Name { get; set; }

        /// <summary>
        /// The specific image name to get.
        /// </summery>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ImageArgumentCompleter))]
        public string[] Id { get; set; }

        /// <summary>
        /// Specifies whether all images should be shown, or just top level images.
        /// </summary>
        [Parameter]
        public SwitchParameter All { get; set; }

        #endregion

        #region Overrides
        /// <summary>
        /// Outputs container image objects for each image matching the provided parameters.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {
            var listParams = new ImagesListParameters() { All = All };

            if (Name != null)
            {
                foreach (var name in Name)
                {
                    listParams.MatchName = name;
                    foreach (var img in await DkrClient.Images.ListImagesAsync(listParams))
                    {
                        WriteObject(img);
                    }
                }
            }
            else
            {
                foreach (var img in await DkrClient.Images.ListImagesAsync(listParams))
                {
                    if (Id == null || Id.Any(i => img.ID.StartsWith(i) || img.ID.StartsWith("sha256:"+i)))
                    {
                        WriteObject(img);
                    }
                }
            }

        }

        #endregion
    }
}
