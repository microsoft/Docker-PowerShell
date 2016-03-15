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

            foreach (var entry in IdMap)
            {
                HostAddress = entry.Value;
                ResetClient();
                var startResult = DkrClient.Containers.StartContainerAsync(
                    entry.Key, new HostConfig());
                AwaitResult(startResult);

                if (!startResult.Result)
                {
                    throw new ApplicationFailedException("The container has already started.");
                }

                if (PassThru.ToBool())
                {
                    var listResponse = DkrClient.Containers.ListContainersAsync(
                        new DotNet.Models.ListContainersParameters() { All = true });
                    AwaitResult(listResponse);
                    Container container =
                        new Container(
                            listResponse.Result.Where(c => entry.Key.Equals(c.Id)).Single(),
                            HostAddress);

                    WriteObject(container);
                }
            }
        }

        #endregion
    }
}
