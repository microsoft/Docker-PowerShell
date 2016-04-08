using System.Management.Automation;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    public class CreateContainerCmdlet : ImageOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// The name to use for the new container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [ValidateNotNullOrEmpty]
        public virtual string ContainerName { get; set; }

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
        public virtual string[] Command { get; set; }

        /// <summary>
        /// The name to use for the new container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [ValidateNotNullOrEmpty]
        public virtual IsolationType Isolation { get; set; }

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
        public virtual Config Configuration { get; set; }

        /// <summary>
        /// The configuration for host settings when running the container.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ConfigObject)]
        public virtual HostConfig HostConfiguration { get; set; }

        #endregion
    }

}
