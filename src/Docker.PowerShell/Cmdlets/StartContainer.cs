using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Start, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class StartContainer : MultiContainerOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// If specified, the resulting output from STDOUT and STDERR will be written to the
        /// console.
        /// </summary>
        [Parameter]
        public SwitchParameter Attach { get; set; }

        /// <summary>
        /// If specified, the container expects to give input to STDIN.
        /// </summary>
        [Parameter]
        public SwitchParameter Input { get; set; }

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
                ContainerAttachParameters attachParams = null;
                if (this.Attach)
                {
                    attachParams = new ContainerAttachParameters
                    {
                        Stdin = this.Input,
                        Stdout = true,
                        Stderr = true,
                        Stream = true
                    };
                }

                var cDetail = await DkrClient.Containers.InspectContainerAsync(id);

                await ContainerOperations.StartContainerAsync(
                    this.DkrClient,
                    id,
                    attachParams,
                    cDetail.Config.Tty,
                    null,
                    this.CmdletCancellationToken);

                if (PassThru)
                {
                    WriteObject((await ContainerOperations.GetContainersByIdOrName(id, DkrClient)).Single());
                }
            }
        }

        #endregion
    }
}
