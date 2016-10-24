using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RemoveContainerImage : ImageOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// Whether or not to force the removal of the image.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetImageIds(Image, ImageIdOrName))
            {
                await DkrClient.Images.DeleteImageAsync(id,
                    new ImageDeleteParameters() { Force = Force.ToBool() }
                    );
            }
        }

        #endregion
    }
}
