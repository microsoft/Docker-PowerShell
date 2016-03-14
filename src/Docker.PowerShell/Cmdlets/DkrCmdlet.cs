using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;

namespace Docker.PowerShell
{
    /// <summary>
    /// Contains strings commonly used as parameter set names.
    /// </summary>
    internal class CommonParameterSetNames
    {
        public const string Default = "Default";
        public const string ContainerObject = "ContainerObject";
    }

    public class DkrCmdlet : PSCmdlet
    {
        #region Private members

        protected DockerClient DkrClient;
        protected string ApiVersion = "1.23";

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
        /// Special WriteObject override that also takes host address and version,
        /// so that they can be dynamically added to the psobject generated from
        /// object being written.
        /// </summary>
        /// <param name="o">The object to write to the pipeline.</param>
        /// <param name="hostAddress">The host address that the object came from.</param>
        /// <param name="apiVersion">The api version used to get the object.</param>
        protected void WriteObject(object o, string hostAddress, string apiVersion)
        {
            var po = PSObject.AsPSObject(o);
            po.Properties.Add(new PSNoteProperty("HostAddress", hostAddress));
            po.Properties.Add(new PSNoteProperty("ApiVersion", apiVersion));
            WriteObject(po);
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
