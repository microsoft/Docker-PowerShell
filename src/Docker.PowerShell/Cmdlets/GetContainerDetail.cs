using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerDetail",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerInspectResponse))]
    public class GetContainerDetail : MultiContainerOperationCmdlet
    {
        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetContainerIds(Container, ContainerIdOrName))
            {
                WriteObject(await DkrClient.Containers.InspectContainerAsync(id));
            }
        }

        #endregion
    }
}
