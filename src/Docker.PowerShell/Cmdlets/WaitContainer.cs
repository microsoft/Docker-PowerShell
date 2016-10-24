using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Wait, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class WaitContainer : MultiContainerOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// If specified, the resulting container object will be output after the operation has
        /// finished.
        /// </summary>
        [Parameter]
        public SwitchParameter PassThru { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetContainerIds(Container, ContainerIdOrName))
            {
                var waitResponse = await DkrClient.Containers.WaitContainerAsync(
                    id,
                    CmdletCancellationToken);

                WriteVerbose("Status Code: " + waitResponse.StatusCode.ToString());
                ContainerOperations.ThrowOnProcessExitCode(waitResponse.StatusCode);

                if (PassThru)
                {
                    WriteObject((await ContainerOperations.GetContainersByIdOrName(id, DkrClient)).Single());
                }
            }
        }

        #endregion
    }
}
