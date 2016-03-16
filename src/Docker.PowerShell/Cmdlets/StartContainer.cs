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

                if (!DkrClient.Containers.StartContainerAsync(
                        entry.Key, new HostConfig()).AwaitResult())
                {
                    throw new ApplicationFailedException("The container has already started.");
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
