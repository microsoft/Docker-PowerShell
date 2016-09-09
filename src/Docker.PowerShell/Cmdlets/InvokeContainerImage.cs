using System;
using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;
using Docker.PowerShell.Support;

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
                    MultiplexedStream stream = null;
                    Task streamTask = null;
                    try
                    {
                        if (!Detach)
                        {
                            var parameters = new ContainerAttachParameters
                            {
                                Stdin = Input,
                                Stdout = true,
                                Stderr = true,
                                Stream = true
                            };

                            stream = await DkrClient.Containers.AttachContainerAsync(createResult.ID, Terminal, parameters, CmdletCancellationToken);
                            streamTask = stream.CopyToConsoleAsync(Terminal, Input, CmdletCancellationToken);
                        }

                        if (!await DkrClient.Containers.StartContainerAsync(createResult.ID, new ContainerStartParameters()))
                        {
                            throw new ApplicationFailedException("The container has already started.");
                        }

                        if (!Detach)
                        {
                            await streamTask;
                        }
                    }
                    finally
                    {
                        stream?.Dispose();
                    }

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
