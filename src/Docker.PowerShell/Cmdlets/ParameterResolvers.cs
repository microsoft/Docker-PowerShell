using System.Collections.Generic;
using System.Linq;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    internal static class ParameterResolvers
    {
        internal struct IdHostPair 
        {
            public string Id;
            public string Host;
        }

        /// <summary>
        /// Translates a list of images or list of ids and a host address into a map of
        /// image id to host address.
        /// </summary>
        /// <param name="images">The list of image objects to get values from.</param>
        /// <param name="ids">The list of ids that should be paired with the given
        /// host address.</param>
        /// <param name="hostAddress">The default host address to pair with the entries
        /// in <paramref name="ids"/>.</param>
        /// <returns></returns>
        internal static List<IdHostPair> GetImageIdMap(Image[] images, string[] ids, string hostAddress)
        {
            var idMap = new List<IdHostPair>();

            if (images != null)
            {
                images.ToList().ForEach(i => idMap.Add(new IdHostPair() { Id = i.ID, Host= i.HostAddress }));
            }
            else
            {
                ids.ToList().ForEach(i => idMap.Add(new IdHostPair() { Id = i, Host = hostAddress }));
            }

            return idMap;
        }

        /// <summary>
        /// Translates a list of containers or list of ids and a host address into a map of
        /// container id to host address.
        /// </summary>
        /// <param name="containers">The list of container objects to get values from.</param>
        /// <param name="ids">The list of ids that should be paired with the given
        /// host address.</param>
        /// <param name="hostAddress">The default host address to pair with the entries
        /// in <paramref name="ids"/>.</param>
        /// <returns></returns>
        internal static List<IdHostPair> GetContainerIdMap(Container[] containers, string[] ids, string hostAddress)
        {
            var idMap = new List<IdHostPair>();

            if (containers != null)
            {
                containers.ToList().ForEach(i => idMap.Add(new IdHostPair() { Id = i.ID, Host = i.HostAddress }));
            }
            else
            {
                ids.ToList().ForEach(i => idMap.Add(new IdHostPair() { Id = i, Host = hostAddress }));
            }

            return idMap;
        }

    }
}
