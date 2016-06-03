using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Container",
            DefaultParameterSetName = CommonParameterSetNames.ContainerName)]
    [OutputType(typeof(ContainerListResponse))]
    public class GetContainer : DkrCmdlet
    {
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ContainerArgumentCompleter))]
        public string[] Id { get; set; }

        [Parameter(ParameterSetName = CommonParameterSetNames.ContainerName,
            ValueFromPipeline = true,
                   Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ContainerArgumentCompleter))]
        public string[] Name { get; set; }

        #region Overrides
        /// <summary>
        /// Outputs container objects for each container matching the provided parameters.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {            
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(await ContainerOperations.GetContainersById(id, DkrClient));    
                }
            }
            else if (Name != null)
            {
                foreach (var name in Name)
                {
                    WriteObject(await ContainerOperations.GetContainersByName(name, DkrClient));
                }
            }
            else
            {
                foreach (var c in await DkrClient.Containers.ListContainersAsync(
                    new ContainersListParameters() { All = true }))
                {
                    WriteObject(c);
                }
            }
        }

        #endregion
    }
}
