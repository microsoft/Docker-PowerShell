using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Stop, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class StopContainer : ContainerOperationCmdlet
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

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                if (Force.ToBool())
                {
                    DkrClient.Containers.KillContainerAsync(
                        id,
                        new ContainerKillParameters()).WaitUnwrap();
                }
                else
                {
                    if (!DkrClient.Containers.StopContainerAsync(
                            id,
                            new ContainerStopParameters(),
                            CancelSignal.Token).AwaitResult())
                    {
                        throw new ApplicationFailedException("The container has already stopped.");
                    }
                }

                if (PassThru.ToBool())
                {
                    WriteObject(ContainerOperations.GetContainerById(id, DkrClient));
                }
            }
        }

        #endregion
    }
}
