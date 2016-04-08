using System;
using System.Net;
using System.Management.Automation;
using Docker.DotNet;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

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

        protected const string ApiVersion = "1.23";
        protected const string KeyFileName = "key.pfx";
        protected CancellationTokenSource CancelSignal = new CancellationTokenSource();

        protected DockerClient DkrClient
        {
            get
            {
                if (dkrClient == null || !dkrClient.Configuration.EndpointBaseUri.ToString().Equals(hostAddress))
                {
                    Credentials cred = null;
                    if (!String.IsNullOrEmpty(CertificateLocation))
                    {
                        //BUGBUG(swernli) - Remove this later in favor of something better.
                        ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                         
                        // Try to find a certificate for secure connections.
                        cred = new DotNet.X509.CertificateCredentials(
                                new X509Certificate2(
                                    System.IO.Path.Combine(CertificateLocation, KeyFileName), 
                                    certPass));
                    }
                    
                    dkrClient = new DockerClientConfiguration(new Uri(HostAddress), cred).CreateClient(new Version(ApiVersion));
                }

                return dkrClient;
            }
        }

        private string hostAddress;
        private string certLoc;
        private string certPass = "p@ssw0rd";
        private DockerClient dkrClient;

        #endregion

        #region Parameters

        /// <summary>
        /// The common parameter for specifying the address of the host to operate on.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [ValidateNotNullOrEmpty]
        public virtual string HostAddress 
        {
            get
            {
                if (String.IsNullOrEmpty(hostAddress))
                {
                    hostAddress = Environment.GetEnvironmentVariable("DOCKER_HOST");
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
        
        ///<summary>
        /// The common parameter for specifying the location to find certificates for use in secure
        /// connections.
        ///</summary>
        [Parameter]
        [ValidateNotNullOrEmpty]
        public virtual string CertificateLocation 
        {
            get 
            {
                if (String.IsNullOrEmpty(certLoc))
                {
                    certLoc = Environment.GetEnvironmentVariable("DOCKER_CERT_PATH");
                }
                
                return certLoc;
            }
            set
            {
                certLoc = value;
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
