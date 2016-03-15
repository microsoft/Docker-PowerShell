using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    public class ImageOperationCmdlet : DkrCmdlet
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
        public virtual Image[] Image { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            IdMap = new Dictionary<string, string>();

            if (Image != null)
            {
                Image.ToList().ForEach(i => IdMap.Add(i.Id, i.HostAddress));
            }
            else
            {
                Id.ToList().ForEach(i => IdMap.Add(i, HostAddress));
            }
        }

        #endregion
    }
}
