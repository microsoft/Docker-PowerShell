using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;

namespace Docker.PowerShell
{
    [Cmdlet("Get", "ContainerImage")]
    public class GetContainerImage : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// Specifies whether all images should be shown, or just top level images.
        /// </summary>
        [Parameter]
        public virtual SwitchParameter All
        {
            get;
            set;
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Outputs container image objects for each image matching the provided parameters.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var img in DkrClient.Images.ListImagesAsync(
                new DotNet.Models.ListImagesParameters() { All = All.ToBool() }).Result)
            {
                WriteObject(img, HostAddress, ApiVersion);
            }
        }

        #endregion
    }
}
