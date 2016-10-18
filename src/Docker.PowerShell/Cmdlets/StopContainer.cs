using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Stop, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class StopContainer : MultiContainerOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// Whether or not to force the termination of the container.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// If specified, the resulting container object will be output after it has finished
        /// starting.
        /// </summary>
        [Parameter]
        public SwitchParameter PassThru { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetContainerIds(Container, ContainerIdOrName))
            {
                if (Force)
                {
                    await DkrClient.Containers.KillContainerAsync(
                        id,
                        new ContainerKillParameters());
                }
                else
                {
                    if (!await DkrClient.Containers.StopContainerAsync(
                            id,
                            new ContainerStopParameters(),
                            CmdletCancellationToken))
                    {
                        throw new ApplicationFailedException("The container has already stopped.");
                    }
                }

                if (PassThru)
                {
                    WriteObject((await ContainerOperations.GetContainersByIdOrName(id, DkrClient)).Single());
                }
            }
        }

        #endregion
    }
}
