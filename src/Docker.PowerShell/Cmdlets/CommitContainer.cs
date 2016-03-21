using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Commit", "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    public class CommitContainer : ContainerOperationCmdlet
    {
        #region Parameters

        [Parameter]
        public string Repository { get; set; }

        [Parameter]
        public string Tag { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string Author { get; set; }

        [Parameter]
        public Config Configuration { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in ParameterResolvers.GetContainerIdMap(Container, Id, HostAddress))
            {
                HostAddress = entry.Value;

                var commitResult = DkrClient.Miscellaneous.CommitContainerChangesAsync(
                    new DotNet.Models.CommitContainerChangesParameters() {
                        ContainerId = entry.Key,
                        Repo = Repository,
                        Tag = Tag,
                        Message = Message,
                        Author = Author,
                        Config = Configuration
                    }).AwaitResult();

                WriteObject(ContainerOperations.GetImageById(commitResult.Id, DkrClient));
            }
        }

        #endregion
    }
}
