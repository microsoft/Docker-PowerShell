using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Run", "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RunContainerImage : CreateContainerCmdlet
    {
        #region Parameters

        /// <summary>
        /// The configuration for host settings when running the container.
        /// </summary>
        [Parameter]
        public virtual HostConfig HostConfiguration { get; set; }

        /// <summary>
        /// If specified, the resulting container will get deleted after it has finished
        /// running.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter RemoveAutomatically { get; set; }

        /// <summary>
        /// If specified, the resulting container object will be output after it has finished
        /// running.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter PassThru { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Creates a new container and lists it to output.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in IdMap)
            {
                var createResult = CreateContainer(entry.Value, entry.Key);
                
                if (createResult.Warnings != null)
                {
                    foreach (var w in createResult.Warnings)
                    {
                        if (!String.IsNullOrEmpty(w))
                        {
                            WriteWarning(w);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(createResult.Id))
                {
                    var startResult = DkrClient.Containers.StartContainerAsync(
                        createResult.Id, HostConfiguration);
                    AwaitResult(startResult);

                    if (!startResult.Result)
                    {
                        throw new ApplicationFailedException("The container has already started.");
                    }

                    var waitResult = DkrClient.Containers.WaitContainerAsync(
                        createResult.Id, 
                        CancelSignal.Token);
                    AwaitResult(waitResult);

                    WriteVerbose("Status Code: " + waitResult.Result.StatusCode.ToString());

                    if (RemoveAutomatically.ToBool())
                    {
                        AwaitResult(DkrClient.Containers.RemoveContainerAsync(createResult.Id,
                            new DotNet.Models.RemoveContainerParameters()));
                    }
                    else if (PassThru.ToBool())
                    {
                        var listResponse = DkrClient.Containers.ListContainersAsync(
                            new DotNet.Models.ListContainersParameters() { All = true });
                        AwaitResult(listResponse);
                        Container container =
                            new Container(
                                listResponse.Result.Where(c => createResult.Id.Equals(c.Id)).Single(),
                                HostAddress);

                        WriteObject(container);
                    }
                }
            }
        }

        #endregion
    }
}
