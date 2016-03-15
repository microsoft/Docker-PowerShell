using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class NewContainer : CreateContainerCmdlet
    {
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
                    var t = DkrClient.Containers.ListContainersAsync(
                        new DotNet.Models.ListContainersParameters() { All = true });
                    AwaitResult(t);
                    Container container = 
                        new Container(t.Result.Where(c => createResult.Id.Equals(c.Id)).Single(), 
                            HostAddress);

                    WriteObject(container);
                }
            }
        }

        #endregion
    }
}
