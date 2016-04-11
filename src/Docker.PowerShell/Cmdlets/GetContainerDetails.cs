using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerDetails",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerInspectResponse))]
    public class GetContainerDetails : ContainerOperationCmdlet
    {
        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                WriteObject(DkrClient.Containers.InspectContainerAsync(id).AwaitResult());
            }
        }

        #endregion
    }
}
