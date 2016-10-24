using System;
using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ContainerListResponse))]
    public class NewContainer : CreateContainerCmdlet
    {
        #region Overrides

        /// <summary>
        /// Creates a new container and lists it to output.
        /// </summary>
        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetImageIds(Image, ImageIdOrName))
            {
                var createResult = await ContainerOperations.CreateContainer(
                    id,
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
                    WriteObject((await ContainerOperations.GetContainersById(createResult.ID, DkrClient)).Single());
                }
            }
        }

        #endregion
    }
}
