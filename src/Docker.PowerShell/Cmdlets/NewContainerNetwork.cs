using System.Management.Automation;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System;
using Docker.PowerShell.Objects;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "ContainerNet",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(NetworkListResponse))]
    public class NewContainerNet : DkrCmdlet
    {
        #region Parameters

        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name;

        [Parameter(Position = 1)]
        [ValidateNotNullOrEmpty]
        public string Driver;

        [Parameter]
        public SwitchParameter Internal;

        [Parameter]
        public SwitchParameter CheckDuplicate;

        [Parameter]
        public SwitchParameter EnableIPv6;

        [Parameter]
        public IPAM IPAM;

        [Parameter]
        public IDictionary<string, string> Options;

        [Parameter]
        public IDictionary<string, string> Labels;

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            var createResult = await DkrClient.Networks.CreateNetworkAsync(new NetworksCreateParameters()
            {
                Name = Name,
                Driver = Driver,
                Internal = Internal,
                CheckDuplicate = CheckDuplicate,
                EnableIPv6 = EnableIPv6,
                IPAM = IPAM,
                Options = Options,
                Labels = Labels
            });

            if (!String.IsNullOrEmpty(createResult.Warning))
            {
                WriteWarning(createResult.Warning);
            }

            if (!String.IsNullOrEmpty(createResult.ID))
            {
                WriteObject(await NetworkOperations.GetNetworksById(createResult.ID, DkrClient));
            }
        }

        #endregion
    }
}