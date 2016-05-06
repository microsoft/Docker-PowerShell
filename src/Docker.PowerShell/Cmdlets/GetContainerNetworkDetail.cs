using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerNetworkDetail")]
    [OutputType(typeof(NetworkResponse))]
    public class GetContainerNetworkDetail : DkrCmdlet
    {
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            var n = await DkrClient.Networks.InspectNetworkAsync(Name);
            this.WriteObject(n);
        }
    }
}