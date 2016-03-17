using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.PowerShell.Objects;

namespace Docker.PowerShell.Cmdlets
{
    internal static class ParameterResolvers
    {
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
        internal static Dictionary<string, string> GetImageIdMap(Image[] images, string[] ids, string hostAddress)
        {
            var idMap = new Dictionary<string, string>();

            if (images != null)
            {
                images.ToList().ForEach(i => idMap.Add(i.Id, i.HostAddress));
            }
            else
            {
                ids.ToList().ForEach(i => idMap.Add(i, hostAddress));
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
        internal static Dictionary<string, string> GetContainerIdMap(Container[] containers, string[] ids, string hostAddress)
        {
            var idMap = new Dictionary<string, string>();

            if (containers != null)
            {
                containers.ToList().ForEach(i => idMap.Add(i.Id, i.HostAddress));
            }
            else
            {
                ids.ToList().ForEach(i => idMap.Add(i, hostAddress));
            }

            return idMap;
        }

    }
}
