using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;

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
                new Docker.DotNet.Models.ListContainersParameters() { All = true }).Result)
            {
                WriteObject(c);
            }
        }

        #endregion
    }
}
