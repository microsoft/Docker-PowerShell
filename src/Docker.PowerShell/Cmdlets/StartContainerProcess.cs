using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using Docker.PowerShell.Support;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Start, "ContainerProcess",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    [Alias("Exec-Container")]
    public class StartContainerProcess : SingleContainerOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// The command to use by default when starting new container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromRemainingArguments = true,
            Position = 1)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ContainerObject,
            ValueFromRemainingArguments = true,
            Position = 1)]
        [ValidateNotNullOrEmpty]
        public string[] Command { get; set; }

        /// <summary>
        /// Whether or not to start the process in detached mode.
        /// </summary>
        [Parameter]
        public SwitchParameter Detached { get; set; }

        /// <summary>
        /// Whether or not to use stdin of the started process.
        /// </summary>
        [Parameter]
        public SwitchParameter Input { get; set; }

        /// <summary>
        /// Whether or not to use terminal emulation.
        /// </summary>
        [Parameter]
        public SwitchParameter Terminal { get; set; }

        /// <summary>
        /// Whether or not to start the process in privileged mode.
        /// </summary>
        [Parameter]
        public SwitchParameter Privileged { get; set; }

        /// <summary>
        /// The user context under which the process should be started.
        /// </summary>
        [Parameter]
        public string User { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            var id = ContainerIdOrName ?? Container.ID;

            var execConfig = new ExecConfig()
            {
                Cmd = Command,
                Privileged = Privileged,
                User = User,
                AttachStdin = !Detached && Input,
                AttachStdout = !Detached,
                AttachStderr = !Detached,
                Detach = Detached,
                Tty = Terminal,
            };

            var procCreateResponse = await DkrClient.Containers.ExecCreateContainerAsync(id, new ContainerExecCreateParameters(execConfig));

            if (Detached)
            {
                await DkrClient.Containers.StartContainerExecAsync(procCreateResponse.ID, CmdletCancellationToken);
                WriteObject(await DkrClient.Containers.InspectContainerExecAsync(procCreateResponse.ID, CmdletCancellationToken));
            }
            else
            {
                using (var stream = await DkrClient.Containers.StartWithConfigContainerExecAsync(procCreateResponse.ID, execConfig, CmdletCancellationToken))
                {
                    await stream.CopyToConsoleAsync(Terminal, Input, CmdletCancellationToken);
                }
            }
        }

        #endregion
    }
}
