using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class GetContainerImage : DkrCmdlet
    {
        #region Parameters

        /// <summary>
        /// Specifies whether all images should be shown, or just top level images.
        /// </summary>
        [Parameter(ParameterSetName = CommonParameterSetNames.Default)]
        public virtual SwitchParameter All { get; set; }

        #endregion

        #region Overrides
        /// <summary>
        /// Outputs container image objects for each image matching the provided parameters.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var t = DkrClient.Images.ListImagesAsync(
                new DotNet.Models.ListImagesParameters() { All = All.ToBool() });
            AwaitResult(t);

            foreach (var img in t.Result)
            {
                WriteObject(new Image(img, HostAddress));
            }
        }

        #endregion
    }
}
