using System.Management.Automation;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Stop", "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
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

            foreach (var entry in ParameterResolvers.GetContainerIdMap(Container, Id, HostAddress))
            {
                HostAddress = entry.Host;

                if (Force.ToBool())
                {
                    DkrClient.Containers.KillContainerAsync(
                        entry.Id,
                        new DotNet.Models.ContainerKillParameters()).WaitUnwrap();
                }
                else
                {
                    if (!DkrClient.Containers.StopContainerAsync(
                            entry.Id,
                            new DotNet.Models.ContainerStopParameters(),
                            CancelSignal.Token).AwaitResult())
                    {
                        throw new ApplicationFailedException("The container has already stopped.");
                    }
                }

                if (PassThru.ToBool())
                {
                    WriteObject(ContainerOperations.GetContainerById(entry.Id, DkrClient));
                }
            }
        }

        #endregion
    }
}
