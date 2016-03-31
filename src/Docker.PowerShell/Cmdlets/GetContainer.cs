using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
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
                new DotNet.Models.ContainersListParameters() { All = true }).AwaitResult())
            {
                WriteObject(new Container(c, HostAddress));
            }
        }

        #endregion
    }
}
