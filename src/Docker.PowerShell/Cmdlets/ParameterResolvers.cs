using System.Linq;
using Docker.DotNet.Models;

namespace Docker.PowerShell.Cmdlets
{
    internal static class ParameterResolvers
    {
        /// <summary>
        /// Uses either the list of IDs, or gets the list of IDs from the list of Images.
        /// </summary>
        /// <param name="images">The list of image objects to get values from.</param>
        /// <param name="ids">The list of ids.</param>
        /// <returns>List of IDs to process.</returns>
        internal static string[] GetImageIds(ImagesListResponse[] images, string[] ids)
        {
            if (ids != null && ids.Length != 0)
            {
                return ids;
            }
            
            return images.Select(i => i.ID).ToArray();
        }

        /// <summary>
        /// Uses either the list of IDs, or gets the list of IDs from the list of containers.
        /// </summary>
        /// <param name="conatiners">The list of container objects to get values from.</param>
        /// <param name="ids">The list of ids.</param>
        /// <returns>List of IDs to process.</returns>
        internal static string[] GetContainerIds(ContainerListResponse[] containers, string[] ids)
        {
            if (ids != null && ids.Length != 0)
            {
                return ids;
            }
            
            return containers.Select(c => c.ID).ToArray();
        }

        /// <summary>
        /// Uses either the list of IDs, or gets the list of IDs from the list of networks.
        /// </summary>
        /// <param name="networks">The list of network objects to get values from.</param>
        /// <param name="ids">The list of ids.</param>
        /// <returns>List of IDs to process.</returns>
        internal static string[] GetNetworkIds(NetworkListResponse[] networks, string[] ids)
        {
            if (ids != null && ids.Length != 0)
            {
                return ids;
            }
            
            return networks.Select(c => c.ID).ToArray();
        }

    }
}
