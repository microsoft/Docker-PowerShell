using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerDetails",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerInspectResponse))]
    public class GetContainerDetails : ContainerOperationCmdlet
    {
        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                WriteObject(await DkrClient.Containers.InspectContainerAsync(id));
            }
        }

        #endregion
    }
}
