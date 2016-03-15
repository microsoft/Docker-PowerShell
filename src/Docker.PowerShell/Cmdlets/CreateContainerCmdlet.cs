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

        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ImageObject)]
        [Parameter(ParameterSetName = CommonParameterSetNames.ConfigObject,
            ValueFromPipeline = true,
            Position = 0,
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public virtual Config Configuration { get; set; }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates a new configuration at the requested host from the given image id.
        /// </summary>
        /// <param name="hostAddress">The address to make the create call to.</param>
        /// <param name="id">The identifier for the image to use in creating the container.</param>
        /// <returns></returns>
        protected DotNet.Models.CreateContainerResponse CreateContainer(string hostAddress, string id)
        {
            if (Configuration == null)
            {
                Configuration = new Config();
            }

            if (!String.IsNullOrEmpty(id))
            {
                Configuration.Image = id;
            }

            if (Command != null)
            {
                Configuration.Cmd = Command;
            }

            HostAddress = hostAddress;
            ResetClient();

            var t = DkrClient.Containers.CreateContainerAsync(
                new DotNet.Models.CreateContainerParameters()
                {
                    ContainerName = ContainerName,
                    Config = Configuration
                });
            AwaitResult(t);

            return t.Result;
        }
        
        #endregion
    }

}
