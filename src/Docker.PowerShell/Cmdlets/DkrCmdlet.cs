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
        protected Uri HostUri;
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

        #endregion

        #region Overrides

        /// <summary>
        /// Common ProcessRecord code for all cmdlets.  Tries to create a client configuration
        /// from the HostAddress, falling back to localhost + default port if none was specified.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (!String.IsNullOrEmpty(HostAddress))
            {
                HostUri = new Uri(HostAddress);
            }
            else
            {
                HostUri = new Uri("http://127.0.0.1:2375");
            }

            DkrClient = new DockerClientConfiguration(HostUri).CreateClient();
        }

        #endregion
    }
}
