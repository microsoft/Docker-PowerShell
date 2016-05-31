using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    public abstract class CreateContainerCmdlet : ImageOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// The name to use for the new container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// The command to use by default when starting new container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromRemainingArguments = true,
            Position = 1)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject,
            ValueFromRemainingArguments = true,
            Position = 1)]
        [ValidateNotNullOrEmpty]
        public string[] Command { get; set; }

        /// <summary>
        /// The name to use for the new container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [ValidateNotNullOrEmpty]
        public IsolationType Isolation { get; set; }

        /// <summary>
        /// The advanced configuration to use for the created container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ConfigObject,
            ValueFromPipeline = true,
            Position = 0,
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public Config Configuration { get; set; }

        /// <summary>
        /// The configuration for host settings when running the container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ConfigObject)]
        public HostConfig HostConfiguration { get; set; }

        /// <summary>
        /// Keep the input pipe of the container open.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        public SwitchParameter Input { get; set; }

        /// <summary>
        /// Allocate a terminal for the container's process so that it can
        /// be used interactively.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        public SwitchParameter Terminal { get; set; }

        #endregion
    }

}
