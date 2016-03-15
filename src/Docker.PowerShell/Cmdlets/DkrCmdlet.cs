using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        protected DockerClient DkrClient;
        protected string ApiVersion = "1.23";
        protected CancellationTokenSource CancelSignal = new CancellationTokenSource();

        #endregion

        #region Parameters

        /// <summary>
        /// The common parameter for specifying the address of the host to operate on.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [ValidateNotNullOrEmpty]
        public virtual string HostAddress { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Common ProcessRecord code for all cmdlets.  Tries to create a client configuration
        /// from the HostAddress, falling back to localhost + default port if none was specified.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (String.IsNullOrEmpty(HostAddress))
            {
                HostAddress = Environment.GetEnvironmentVariable("DOCKER_HOST");
                if (String.IsNullOrEmpty(HostAddress))
                {
                    HostAddress = "http://127.0.0.1:2375";
                }
            }

            ResetClient();
        }

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

        #region Helpers

        /// <summary>
        /// Helper method that creates a new docker client based on the currently configured
        /// HostAddress and ApiVersion and sets the DkrClient member variable to it.
        /// </summary>
        protected void ResetClient()
        {
            DkrClient = new DockerClientConfiguration(new Uri(HostAddress)).CreateClient(new Version(ApiVersion));
        }

        protected void AwaitResult(Task t)
        {
            try
            {
                t.Wait();
            }
            catch (System.Exception ex)
            {
                throw ex.InnerException;
            }
        }

        #endregion
    }
}
