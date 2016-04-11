using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ImagesListResponse))]
    public class GetContainerImage : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// Specifies whether all images should be shown, or just top level images.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        public SwitchParameter All { get; set; }

        #endregion

        #region Overrides
        /// <summary>
        /// Outputs container image objects for each image matching the provided parameters.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var img in DkrClient.Images.ListImagesAsync(
                new ImagesListParameters() { All = All.ToBool() }).AwaitResult())
            {
                WriteObject(img);
            }
        }

        #endregion
    }
}
