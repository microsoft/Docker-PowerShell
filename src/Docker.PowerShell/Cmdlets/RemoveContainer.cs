using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RemoveContainer : ContainerOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// Whether or not to force the removal of the container.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter Force { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in IdMap)
            {
                HostAddress = entry.Value;
                ResetClient();
                AwaitResult(DkrClient.Containers.RemoveContainerAsync(entry.Key,
                    new DotNet.Models.RemoveContainerParameters() { Force = Force.ToBool() }
                    ));
            }
        }

        #endregion
    }
}
