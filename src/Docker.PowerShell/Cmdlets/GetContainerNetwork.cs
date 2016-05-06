using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerNetwork")]
    [OutputType(typeof(NetworkListResponse))]
    public class GetContainerNetwork : DkrCmdlet
    {
        protected override async Task ProcessRecordAsync()
        {
            // TODO: Pass the filters?
            foreach (var n in await DkrClient.Networks.ListNetworksAsync())
            {
                this.WriteObject(n);
            }
        }
    }
}