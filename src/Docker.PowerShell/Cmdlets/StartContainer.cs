using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Start, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class StartContainer : MultiContainerOperationCmdlet
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

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                if (!await DkrClient.Containers.StartContainerAsync(
                        id, new HostConfig()))
                {
                    throw new ApplicationFailedException("The container has already started.");
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
