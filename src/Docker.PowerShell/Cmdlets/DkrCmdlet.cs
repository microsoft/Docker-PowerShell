using System.Management.Automation;
using Docker.DotNet;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    /// <summary>
    /// Contains strings commonly used as parameter set names.
    /// </summary>
    internal class CommonParameterSetNames
    {
        public const string Default = "Default";
        public const string ContainerObject = "ContainerObject";
        public const string ImageObject = "ImageObject";
        public const string ConfigObject = "ConfigObject";
    }

    public abstract class DkrCmdlet : PSCmdlet
    {
        #region Private members

        private CancellationTokenSource CancelSignal = new CancellationTokenSource();

        protected CancellationToken CmdletCancellationToken => CancelSignal.Token;

        protected DockerClient DkrClient
        {
            get
            {
                if (dkrClient == null)
                {
                    dkrClient = DockerFactory.CreateClient(HostAddress, CertificateLocation);
                }

                return dkrClient;
            }
        }

        private DockerClient dkrClient;

        #endregion

        #region Parameters

        /// <summary>
        /// The common parameter for specifying the address of the host to operate on.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [ValidateNotNullOrEmpty]
        public string HostAddress { get; set; }

        ///<summary>
        /// The common parameter for specifying the location to find certificates for use in secure
        /// connections.
        ///</summary>
        [Parameter]
        [ValidateNotNullOrEmpty]
        public string CertificateLocation { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Common StopProcessing code, that signals the CancellationToken. This may or may
        /// not be used be child classes in http calls to docker.
        /// </summary>
        protected override void StopProcessing()
        {
            CancelSignal.Cancel();
        }

        protected sealed override void ProcessRecord()
        {
            AsyncPump.Run(ProcessRecordAsync);
        }

        protected abstract Task ProcessRecordAsync();

        #endregion
    }
}
