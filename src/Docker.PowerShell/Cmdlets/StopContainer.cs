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

                var stopResult = DkrClient.Containers.StopContainerAsync(
                    entry.Key, 
                    new DotNet.Models.StopContainerParameters(),
                    CancelSignal.Token);
                AwaitResult(stopResult);

                if (!stopResult.Result)
                {
                    throw new ApplicationFailedException("The container has already stopped.");
                }

                if (PassThru.ToBool())
                {
                    var listResponse = DkrClient.Containers.ListContainersAsync(
                        new DotNet.Models.ListContainersParameters() { All = true });
                    AwaitResult(listResponse);
                    Container container =
                        new Container(
                            listResponse.Result.Where(c => c.Id.StartsWith(entry.Key) || c.Names.Any(n => n.Equals("/" + entry.Key))).Single(),
                            HostAddress);

                    WriteObject(container);
                }
            }
        }

        #endregion
    }
}
