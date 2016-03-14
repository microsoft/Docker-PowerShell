using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Remove, "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class RemoveContainer : DkrCmdlet
    {
        #region Members

        internal Dictionary<string, string> IdMap;

        #endregion

        #region Parameters

        /// <summary>
        /// The Ids for which containers to remove.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public virtual string[] Id { get; set; }

        /// <summary>
        /// The containers to remove.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.ContainerObject,
            ValueFromPipeline = true,
                   Position = 0,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public virtual ContainerListResponse[] Container { get; set; }

        /// <summary>
        /// Whether or not to force the removal of the container.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter Force { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            IdMap = new Dictionary<string, string>();

            if (Container != null)
            {
                Container.ToList().ForEach(c => IdMap.Add(c.Id, c.HostAddress));
            }
            else
            {
                Id.ToList().ForEach(i => IdMap.Add(i, HostAddress));
            }

            foreach (var entry in IdMap)
            {
                HostAddress = entry.Value;
                ResetClient();
                DkrClient.Containers.RemoveContainerAsync(entry.Key,
                    new DotNet.Models.RemoveContainerParameters() { Force = Force.ToBool() }
                    );
            }
        }

        #endregion
    }
}
