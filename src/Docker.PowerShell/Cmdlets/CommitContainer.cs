using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet("Commit", "Container",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [OutputType(typeof(ImagesListResponse))]
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

            foreach (var id in ParameterResolvers.GetContainerIds(Container, Id))
            {
                var commitResult = DkrClient.Miscellaneous.CommitContainerChangesAsync(
                    new CommitContainerChangesParameters() {
                        ContainerID = id,
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
