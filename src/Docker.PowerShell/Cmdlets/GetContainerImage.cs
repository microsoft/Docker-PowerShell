using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ImagesListResponse))]
    public class GetContainerImage : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// The specific image name to get.
        /// </summery>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ImageArgumentCompleter))]
        [Alias("ImageName", "ImageId")]
        public string[] ImageIdOrName { get; set; }

        /// <summary>
        /// Specifies whether all images should be shown, or just top level images.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.AllImages)]
        public SwitchParameter All { get; set; }

        #endregion

        #region Overrides
        /// <summary>
        /// Outputs container image objects for each image matching the provided parameters.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {
            var listParams = new ImagesListParameters() { All = (All || ImageIdOrName != null) };

            foreach (var img in await DkrClient.Images.ListImagesAsync(listParams))
            {
                if (ImageIdOrName == null ||
                    ImageIdOrName.Any(i => img.RepoTags.Any(r => i.Split('/').Last().Contains(":") ? r == i : r == (i + ":latest"))) ||
                    ImageIdOrName.Any(i => img.ID.StartsWith(i) || img.ID.StartsWith("sha256:" + i)))
                {
                    WriteObject(img);
                }
            }
        }

        #endregion
    }
}
