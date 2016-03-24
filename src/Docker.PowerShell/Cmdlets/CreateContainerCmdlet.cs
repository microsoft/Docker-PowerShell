using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
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
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [ValidateNotNullOrEmpty]
        public virtual string[] Command { get; set; }

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

        #endregion
    }

}
