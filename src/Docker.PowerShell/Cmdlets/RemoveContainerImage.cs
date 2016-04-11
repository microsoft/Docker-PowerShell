using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RemoveContainerImage : ImageOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// Whether or not to force the removal of the container.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var id in ParameterResolvers.GetImageIds(Image, Id))
            {
                DkrClient.Images.DeleteImageAsync(id,
                    new ImageDeleteParameters() { Force = Force.ToBool() }
                    ).WaitUnwrap();
            }
        }

        #endregion
    }
}
