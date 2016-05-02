using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class GetContainer : DkrCmdlet
    {

        #region Overrides
        /// <summary>
        /// Outputs container objects for each container matching the provided parameters.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {
            foreach (var c in await DkrClient.Containers.ListContainersAsync(
                new ContainersListParameters() { All = true }))
            {
                WriteObject(c);
            }
        }

        #endregion
    }
}
