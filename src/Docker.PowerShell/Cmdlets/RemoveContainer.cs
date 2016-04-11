using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RemoveContainer : ContainerOperationCmdlet
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

            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                DkrClient.Containers.RemoveContainerAsync(id,
                    new ContainerRemoveParameters() { Force = Force.ToBool() }
                    ).WaitUnwrap();
            }
        }

        #endregion
    }
}
