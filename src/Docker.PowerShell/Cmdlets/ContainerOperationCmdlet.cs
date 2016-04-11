using System.Management.Automation;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    public abstract class ContainerOperationCmdlet : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// The Ids for which containers to remove.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string[] Id { get; set; }

        /// <summary>
        /// The containers to remove.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.ContainerObject,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public ContainerListResponse[] Container { get; set; }

        #endregion
    }
}
