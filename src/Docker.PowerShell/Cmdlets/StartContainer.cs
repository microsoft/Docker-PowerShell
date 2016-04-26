using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Start, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class StartContainer : ContainerOperationCmdlet
    {
        #region Parameters

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
                if (!DkrClient.Containers.StartContainerAsync(
                        id, new HostConfig()).AwaitResult())
                {
                    throw new ApplicationFailedException("The container has already started.");
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
