using System.Management.Automation;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace Docker.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "ContainerImageTag",
            DefaultParameterSetName = CommonParameterSetNames.Default)]
    [Alias("Tag-ContainerImage")]
    public class AddContainerImageTag : ImageOperationCmdlet
    {
        #region Parameters

        [Parameter]
        public SwitchParameter Force { get; set; }
        
        [Parameter(Position = 1,
                   Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Repository { get; set; }

        [Parameter(Position = 2)]
        [ValidateNotNullOrEmpty]
        public string Tag { get; set; }

        #endregion

        #region Overrides

        protected override async Task ProcessRecordAsync()
        {
            foreach (var id in ParameterResolvers.GetImageIds(Image, ImageIdOrName))
            {
                var tagParams = new ImageTagParameters(){ RepositoryName = Repository };
                                
                if (!string.IsNullOrEmpty(Tag))
                {
                    tagParams.Tag = Tag;
                }
                
                if (Force)
                {
                    tagParams.Force = true;
                }
                
                await DkrClient.Images.TagImageAsync(id, tagParams);
            }
        }

        #endregion
    }
}
