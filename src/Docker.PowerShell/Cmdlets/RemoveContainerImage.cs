using System.Management.Automation;
using Docker.PowerShell.Objects;

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
        public virtual SwitchParameter Force { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in ParameterResolvers.GetImageIdMap(Image, Id, HostAddress))
            {
                HostAddress = entry.Host;
                DkrClient.Images.DeleteImageAsync(entry.Id,
                    new DotNet.Models.ImageDeleteParameters() { Force = Force.ToBool() }
                    ).WaitUnwrap();
            }
        }

        #endregion
    }
}
