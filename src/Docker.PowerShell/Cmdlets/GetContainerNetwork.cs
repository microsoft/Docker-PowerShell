using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Get", "ContainerNetwork")]
    [OutputType(typeof(NetworkResponse))]
    public class GetContainerNetwork : DkrCmdlet
    {
        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }
        
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            
            if (!string.IsNullOrEmpty(this.Name))
            {
                var n = this.DkrClient.Networks.InspectNetworkAsync(this.Name).AwaitResult();
                this.WriteObject(n);
            }
            else
            {
                // TODO: Pass the filters?
                foreach (var n in this.DkrClient.Networks.ListNetworksAsync().AwaitResult())
                {
                    this.WriteObject(n);
                }
            }
        }
    }
}