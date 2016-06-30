using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerNetDetail")]
    [OutputType(typeof(NetworkResponse))]
    public class GetContainerNetDetail : NetworkOperationCmdlet
    {
        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetNetworkIds(Network, Id))
            {
                var n = await DkrClient.Networks.InspectNetworkAsync(id);
                this.WriteObject(n);
            }
        }
    }
}