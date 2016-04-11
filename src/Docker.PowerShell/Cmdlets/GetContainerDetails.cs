using System.Management.Automation;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerDetails",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class GetContainerDetails : ContainerOperationCmdlet
    {
        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in ParameterResolvers.GetContainerIdMap(Container, Id, HostAddress))
            {
                HostAddress = entry.Host;
                
                WriteObject(DkrClient.Containers.InspectContainerAsync(entry.Id).AwaitResult());
            }
        }

        #endregion
    }
}
