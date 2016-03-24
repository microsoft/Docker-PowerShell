using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Start", "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class StartContainer : ContainerOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// If specified, the resulting container object will be output after it has finished
        /// starting.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter PassThru { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in ParameterResolvers.GetContainerIdMap(Container, Id, HostAddress))
            {
                HostAddress = entry.Host;

                if (!DkrClient.Containers.StartContainerAsync(
                        entry.Id, new HostConfig()).AwaitResult())
                {
                    throw new ApplicationFailedException("The container has already started.");
                }

                if (PassThru.ToBool())
                {
                    WriteObject(ContainerOperations.GetContainerById(entry.Id, DkrClient));
                }
            }
        }

        #endregion
    }
}
