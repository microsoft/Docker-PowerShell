using System.Management.Automation;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "ContainerNet",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RemoveContainerNet : NetworkOperationCmdlet
    {
        #region Parameters

        /// <summary>
        /// Whether or not to force the removal of the image.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetNetworkIds(Network, Id))
            {
                await DkrClient.Networks.DeleteNetworkAsync(id);
            }
        }

        #endregion
    }
}
