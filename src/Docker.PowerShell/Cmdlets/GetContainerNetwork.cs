using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerNet",
            DefaultParameterSetName = CommonParameterSetNames.NetworkName)]
    [OutputType(typeof(NetworkListResponse))]
    public class GetContainerNet : DkrCmdlet
    {
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(NetworkArgumentCompleter))]
        public string[] Id { get; set; }

        [Parameter(ParameterSetName = CommonParameterSetNames.NetworkName,
            ValueFromPipeline = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(NetworkArgumentCompleter))]
        public string[] Name { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(await NetworkOperations.GetNetworksById(id, DkrClient));
                }
            }
            else if (Name != null)
            {
                foreach (var name in Name)
                {
                    WriteObject(await NetworkOperations.GetNetworksByName(name, DkrClient));
                }
            }
            else
            {
                foreach (var n in await DkrClient.Networks.ListNetworksAsync())
                {
                    WriteObject(n);
                }
            }

        }
    }
}