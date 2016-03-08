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

        #region Overrides
        /// <summary>
        /// Outputs container image objects for each image matching the provided parameters.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var img in DkrClient.Images.ListImagesAsync(
                new DotNet.Models.ListImagesParameters() { }).Result)
            {
                WriteObject(img);
            }
        }

        #endregion
    }
}
