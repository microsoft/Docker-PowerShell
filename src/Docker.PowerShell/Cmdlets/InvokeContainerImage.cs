using System;
using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Invoke, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [Alias("Run-ContainerImage")]
    [OutputType(typeof(ContainerListResponse))]
    public class InvokeContainerImage : CreateContainerCmdlet
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
        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetImageIds(Image, Id))
            {
                var createResult = await ContainerOperations.CreateContainer(
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
                    if (!await DkrClient.Containers.StartContainerAsync(
                        createResult.ID, HostConfiguration))
                    {
                        throw new ApplicationFailedException("The container has already started.");
                    }

                    var waitResponse = await DkrClient.Containers.WaitContainerAsync(
                        createResult.ID,
                        CmdletCancellationToken);

                    WriteVerbose("Status Code: " + waitResponse.StatusCode.ToString());
                    ContainerOperations.ThrowOnProcessExitCode(waitResponse.StatusCode);

                    if (RemoveAutomatically.ToBool())
                    {
                        await DkrClient.Containers.RemoveContainerAsync(createResult.ID,
                            new ContainerRemoveParameters());
                    }
                    else if (PassThru.ToBool())
                    {
                        WriteObject((await ContainerOperations.GetContainersById(createResult.ID, DkrClient)).Single());
                    }
                }
            }
        }

        #endregion
    }
}
