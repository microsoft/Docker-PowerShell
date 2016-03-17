using System;
using System.Linq;

namespace Docker.PowerShell.Objects
{
    internal static class ContainerOperations
    {
        /// <summary>
        /// Creates a container from the given inputs.
        /// </summary>
        /// <param name="id">The Id of the image to create the container from.</param>
        /// <param name="configuration">The configuration of the container. May be null.</param>
        /// <param name="command">The command array for the container.</param>
        /// <param name="name">The name for the container.</param>
        /// <param name="dkrClient">The client to use when creating the container.</param>
        /// <returns>The http response object for the create call.</returns>
        internal static DotNet.Models.CreateContainerResponse CreateContainer(
            string id,
            Config configuration,
            string[] command,
            string name,
            DotNet.DockerClient dkrClient)
        {
            if (configuration == null)
            {
                configuration = new Config();
            }

            if (!String.IsNullOrEmpty(id))
            {
                configuration.Image = id;
            }

            if (command != null)
            {
                configuration.Cmd = command;
            }

            return dkrClient.Containers.CreateContainerAsync(
                new DotNet.Models.CreateContainerParameters()
                {
                    ContainerName = name,
                    Config = configuration
                }).AwaitResult();
        }

        /// <summary>
        /// Gets a single container object from the client by id.
        /// </summary>
        /// <param name="id">The container identifier to retrieve.</param>
        /// <param name="dkrClient">The client to request the container from.</param>
        /// <returns>The single container object matching the id.</returns>
        internal static Container GetContainerById(string id, DotNet.DockerClient dkrClient)
        {
            // TODO - Have a better way to get the container list response given the ID.
            return new Container(
                dkrClient.Containers.ListContainersAsync(
                    new DotNet.Models.ListContainersParameters() { All = true }).AwaitResult().Where(
                        c => c.Id.StartsWith(id) || c.Names.Any(n => n.Equals("/" + id))).Single(),
                    dkrClient.Configuration.EndpointBaseUri.ToString());
        }
    }
}
