using System.Management.Automation;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    public abstract class ImageOperationCmdlet : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// The Ids for which containers to remove.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ImageArgumentCompleter))]
        [Alias("ImageName", "ImageId")]
        public string[] ImageIdOrName { get; set; }

        /// <summary>
        /// The containers to remove.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ImagesListResponse[] Image { get; set; }

        #endregion
    }
}
