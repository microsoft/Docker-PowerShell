using System.Management.Automation;
using Docker.PowerShell.Objects;

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

            foreach (var entry in ParameterResolvers.GetContainerIdMap(Container, Id, HostAddress))
            {
                HostAddress = entry.Host;
                DkrClient.Containers.RemoveContainerAsync(entry.Id,
                    new DotNet.Models.ContainerRemoveParameters() { Force = Force.ToBool() }
                    ).WaitUnwrap();
            }
        }

        #endregion
    }
}
