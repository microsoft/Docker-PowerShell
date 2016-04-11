using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Wait", "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class WaitContainer : ContainerOperationCmdlet
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

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                var waitResponse = DkrClient.Containers.WaitContainerAsync(
                    id,
                    CancelSignal.Token).AwaitResult();
                
                WriteVerbose("Status Code: " + waitResponse.StatusCode.ToString());
                ContainerOperations.ThrowOnProcessExitCode(waitResponse.StatusCode);

                if (PassThru.ToBool())
                {
                    WriteObject(ContainerOperations.GetContainerById(id, DkrClient));
                }
            }
        }

        #endregion
    }
}
