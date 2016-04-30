using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerNetwork")]
    [OutputType(typeof(NetworkResponse))]
    public class GetContainerNetwork : DkrCmdlet
    {
        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                var n = await DkrClient.Networks.InspectNetworkAsync(this.Name);
                this.WriteObject(n);
            }
            else
            {
                // TODO: Pass the filters?
                foreach (var n in await DkrClient.Networks.ListNetworksAsync())
                {
                    this.WriteObject(n);
                }
            }
        }
    }
}