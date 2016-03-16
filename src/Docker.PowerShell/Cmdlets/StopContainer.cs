using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Stop", "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class StopContainer : ContainerOperationCmdlet
    {
        #region Parameters
        
        /// <summary>
        /// Whether or not to force the termination of the container.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter Force { get; set; }

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

            foreach (var entry in IdMap)
            {
                HostAddress = entry.Value;
                ResetClient();

                if (Force.ToBool())
                {
                    DkrClient.Containers.KillContainerAsync(
                        entry.Key,
                        new DotNet.Models.KillContainerParameters()).WaitUnwrap();
                }
                else
                {
                    if (!DkrClient.Containers.StopContainerAsync(
                            entry.Key,
                            new DotNet.Models.StopContainerParameters(),
                            CancelSignal.Token).AwaitResult())
                    {
                        throw new ApplicationFailedException("The container has already stopped.");
                    }
                }

                if (PassThru.ToBool())
                {
                    Container container =
                        new Container(
                            DkrClient.Containers.ListContainersAsync(
                                new DotNet.Models.ListContainersParameters() { All = true }).AwaitResult().Where(
                                    c => c.Id.StartsWith(entry.Key) || c.Names.Any(n => n.Equals("/" + entry.Key))).Single(),
                            HostAddress);

                    WriteObject(container);
                }
            }
        }

        #endregion
    }
}
