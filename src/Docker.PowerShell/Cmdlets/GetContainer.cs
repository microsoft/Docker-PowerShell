using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell
{
    [Cmdlet("Get", "Container")]
    public class GetContainer : DkrCmdlet
    {

        #region Overrides
        /// <summary>
        /// Outputs container objects for each container matching the provided parameters.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var c in DkrClient.Containers.ListContainersAsync(
                new DotNet.Models.ListContainersParameters() { All = true }).Result)
            {
                WriteObject(new Objects.ContainerListResponse(c, HostAddress, ApiVersion));
            }
        }

        #endregion
    }
}
