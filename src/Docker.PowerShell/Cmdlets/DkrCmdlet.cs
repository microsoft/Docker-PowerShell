using System;
using System.Management.Automation;
using Docker.DotNet;
using System.Threading;

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

    public class DkrCmdlet : PSCmdlet
    {
        #region Private members

        protected string ApiVersion = "1.23";
        protected CancellationTokenSource CancelSignal = new CancellationTokenSource();

        protected DockerClient DkrClient
        {
            get
            {
                if (dkrClient == null || !dkrClient.Configuration.EndpointBaseUri.ToString().Equals(hostAddress))
                {
                    dkrClient = new DockerClientConfiguration(new Uri(HostAddress)).CreateClient(new Version(ApiVersion));
                }

                return dkrClient;
            }
        }

        private string hostAddress;
        private DockerClient dkrClient;

        #endregion

        #region Parameters

        /// <summary>
        /// The common parameter for specifying the address of the host to operate on.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [ValidateNotNullOrEmpty]
        public virtual string HostAddress {
            get
            {
                if (String.IsNullOrEmpty(hostAddress))
                {
                    HostAddress = Environment.GetEnvironmentVariable("DOCKER_HOST");
                    if (String.IsNullOrEmpty(hostAddress))
                    {
                        hostAddress = "http://127.0.0.1:2375";
                    }
                }

                return hostAddress;
            }
            set
            {
                hostAddress = value;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Common StopProcessing code, that signals the CancellationToken. This may or may
        /// not be used be child classes in http calls to docker.
        /// </summary>
        protected override void StopProcessing()
        {
            base.StopProcessing();

            CancelSignal.Cancel();
        }

        #endregion
    }
}
