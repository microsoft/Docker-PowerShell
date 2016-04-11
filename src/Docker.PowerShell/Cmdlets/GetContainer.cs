using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

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
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var c in DkrClient.Containers.ListContainersAsync(
                new ContainersListParameters() { All = true }).AwaitResult())
            {
                WriteObject(c);
            }
        }

        #endregion
    }
}
