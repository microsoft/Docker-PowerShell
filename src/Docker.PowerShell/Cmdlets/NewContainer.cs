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

            foreach (var entry in ParameterResolvers.GetImageIdMap(Image, Id, HostAddress))
            {
                HostAddress = entry.Host;
                var createResult = ContainerOperations.CreateContainer(
                    entry.Id,
                    this.MemberwiseClone() as CreateContainerCmdlet,
                    DkrClient);
                
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

                if (!String.IsNullOrEmpty(createResult.ID))
                {
                    WriteObject(ContainerOperations.GetContainerById(createResult.ID, DkrClient));
                }
            }
        }

        #endregion
    }
}
