using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;

namespace Docker.PowerShell
{
    public class DkrCmdlet : PSCmdlet
    {
        #region Private members

        // The Uri generated from HostAddress that will be used for remote connections.
        protected DockerClient DkrClient;

        #endregion

        #region Parameters

        /// <summary>
        /// The common parameter for specifying the address of the host to operate on.
        /// </summary>
        [Parameter]
        [ValidateNotNullOrEmpty]
        public virtual string HostAddress
        {
            get;
            set;
        }

        /// <summary>
        /// The common parameter for specifying the version that should be used
        /// when communicating with the host.
        /// </summary>
        [Parameter]
        [ValidateNotNullOrEmpty]
        public virtual string ApiVersion
        {
            get;
            set;
        }

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

            if (String.IsNullOrEmpty(ApiVersion))
            {
                ApiVersion = Environment.GetEnvironmentVariable("DOCKER_API_VERSION");
                if (String.IsNullOrEmpty(ApiVersion))
                {
                    ApiVersion = "1.23";
                }
            }

            ResetClient();
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

        #endregion
    }
}
