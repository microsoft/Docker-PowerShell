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

        /// <summary>
        /// The repository name for the created image.
        /// </summary>
        [Parameter]
        public string Repository { get; set; }

        /// <summary>
        /// The tag name for the created image.
        /// </summary>
        [Parameter]
        public string Tag { get; set; }

        /// <summary>
        /// A message to be associated with the created image.
        /// </summary>
        [Parameter]
        public string Message { get; set; }

        /// <summary>
        /// The author of the created image.
        /// </summary>
        [Parameter]
        public string Author { get; set; }

        /// <summary>
        /// The advanced configuration to be used for the image.
        /// </summary>
        [Parameter]
        public Config Configuration { get; set; }

        #endregion

        #region Overrides

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (var entry in ParameterResolvers.GetContainerIdMap(Container, Id, HostAddress))
            {
                HostAddress = entry.Host;

                var commitResult = DkrClient.Miscellaneous.CommitContainerChangesAsync(
                    new DotNet.Models.CommitContainerChangesParameters() {
                        ContainerID = entry.Id,
                        RepositoryName = Repository,
                        Tag = Tag,
                        Comment = Message,
                        Author = Author,
                        Config = Configuration
                    }).AwaitResult();

                WriteObject(ContainerOperations.GetImageById(commitResult.ID, DkrClient));
            }
        }

        #endregion
    }
}
