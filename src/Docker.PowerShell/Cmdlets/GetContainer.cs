using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class GetContainer : DkrCmdlet
    {
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(ContainerArgumentCompleter))]
        [Alias("Name","Id")]
        public string[] ContainerIdOrName { get; set; }

        #region Overrides
        /// <summary>
        /// Outputs container objects for each container matching the provided parameters.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {
            if (ContainerIdOrName != null)
            {
                foreach (var id in ContainerIdOrName)
                {
                    WriteObject(await ContainerOperations.GetContainersByIdOrName(id, DkrClient));
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
