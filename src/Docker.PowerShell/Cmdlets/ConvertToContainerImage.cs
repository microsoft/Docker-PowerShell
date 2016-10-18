using System.Management.Automation;
using Docker.PowerShell.Objects;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsData.ConvertTo, "ContainerImage",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [Alias("Commit-Container")]
    [OutputType(typeof(ImagesListResponse))]
    public class ConvertToContainerImage : MultiContainerOperationCmdlet
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

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetContainerIds(Container, ContainerIdOrName))
            {
                var commitResult = await DkrClient.Miscellaneous.CommitContainerChangesAsync(
                    new CommitContainerChangesParameters() {
                        ContainerID = id,
                        RepositoryName = Repository,
                        Tag = Tag,
                        Comment = Message,
                        Author = Author,
                        Config = Configuration
                    });

                WriteObject(await ContainerOperations.GetImageById(commitResult.ID, DkrClient));
            }
        }

        #endregion
    }
}
