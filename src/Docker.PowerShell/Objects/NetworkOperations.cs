using System.Collections.Generic;

namespace Docker.PowerShell.Objects
{
    using System.Threading.Tasks;
    using DotNet.Models;

    internal static class NetworkOperations
    {
        internal static Task<IList<NetworkListResponse>> GetNetworksById(string id, DotNet.DockerClient dkrClient)
        {
            return (dkrClient.Networks.ListNetworksAsync(new NetworksListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                        {
                            {"id", new Dictionary<string, bool>
                                {
                                    {id, true}
                                }
                            }
                        }
            }));
        }

        internal static Task<IList<NetworkListResponse>> GetNetworksByName(string name, DotNet.DockerClient dkrClient)
        {
            return (dkrClient.Networks.ListNetworksAsync(new NetworksListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                        {
                            {"name", new Dictionary<string, bool>
                                {
                                    {name, true}
                                }
                            }
                        }
            }));
        }
    }
}