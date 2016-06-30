using System.Management.Automation;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    public abstract class NetworkOperationCmdlet : DkrCmdlet
    {
        #region Parameters

        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        [ArgumentCompleter(typeof(NetworkArgumentCompleter))]
        public string[] Id { get; set; }

        [Parameter(ParameterSetName = CommonParameterSetNames.NetworkObject,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public NetworkListResponse[] Network { get; set; }

        #endregion
    }
}
