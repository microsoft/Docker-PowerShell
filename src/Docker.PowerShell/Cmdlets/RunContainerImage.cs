using System;
using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Run", "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class RunContainerImage : CreateContainerCmdlet
    {
        #region Parameters

        /// <summary>
        /// If specified, the resulting container will get deleted after it has finished
        /// running.
        /// </summary>
        [Parameter]
        public SwitchParameter RemoveAutomatically { get; set; }

        /// <summary>
        /// If specified, the resulting container object will be output after it has finished
        /// running.
        /// </summary>
        [Parameter]
        public SwitchParameter PassThru { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Creates a new container and lists it to output.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var id in ParameterResolvers.GetImageIds(Image, Id))
            {
                var createResult = ContainerOperations.CreateContainer(
                    id,
                    this.MemberwiseClone() as CreateContainerCmdlet,
                    DkrClient);

                if (createResult.Warnings != null)
                {
                    foreach (var w in createResult.Warnings)
                    {
                        if (!String.IsNullOrEmpty(w))
                        {
                            WriteWarning(w);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(createResult.ID))
                {
                    if (!DkrClient.Containers.StartContainerAsync(
                        createResult.ID, HostConfiguration).AwaitResult())
                    {
                        throw new ApplicationFailedException("The container has already started.");
                    }

                    var waitResponse = DkrClient.Containers.WaitContainerAsync(
                        createResult.ID, 
                        CancelSignal.Token).AwaitResult();

                    WriteVerbose("Status Code: " + waitResponse.StatusCode.ToString());
                    ContainerOperations.ThrowOnProcessExitCode(waitResponse.StatusCode);

                    if (RemoveAutomatically.ToBool())
                    {
                        DkrClient.Containers.RemoveContainerAsync(createResult.ID,
                            new ContainerRemoveParameters()).WaitUnwrap();
                    }
                    else if (PassThru.ToBool())
                    {
                        WriteObject(ContainerOperations.GetContainerById(createResult.ID, DkrClient));
                    }
                }
            }
        }

        #endregion
    }
}
