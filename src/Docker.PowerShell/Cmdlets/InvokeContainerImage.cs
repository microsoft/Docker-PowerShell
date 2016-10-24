using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Docker.PowerShell.Objects;

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

        /// <summary>
        /// If specified, the container will run in detached mode without connecting to input/output pipes.
        /// </summary>
        [Parameter]
        public SwitchParameter Detach { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Creates a new container and lists it to output.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetImageIds(Image, ImageIdOrName))
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
                    ContainerAttachParameters attachParams = null;
                    if (!Detach)
                    {
                        attachParams = new ContainerAttachParameters
                        {
                            Stdin = Input,
                            Stdout = true,
                            Stderr = true,
                            Stream = true
                        };
                    }

                    await ContainerOperations.StartContainerAsync(
                        this.DkrClient,
                        createResult.ID,
                        attachParams,
                        this.Terminal,
                        null,
                        this.CmdletCancellationToken);

                    if (RemoveAutomatically && !Detach)
                    {
                        await DkrClient.Containers.RemoveContainerAsync(createResult.ID,
                            new ContainerRemoveParameters());
                    }
                    else if (PassThru)
                    {
                        WriteObject((await ContainerOperations.GetContainersById(createResult.ID, DkrClient)).Single());
                    }
                }
            }
        }

        #endregion
    }
}
