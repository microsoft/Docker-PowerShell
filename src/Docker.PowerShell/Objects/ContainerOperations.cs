using System;
using System.Linq;
using Docker.PowerShell.Cmdlets;

namespace Docker.PowerShell.Objects
{
    using DotNet.Models;

    internal static class ContainerOperations
    {
        /// <summary>
        /// Creates the container 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmdlet"></param>
        /// <param name="dkrClient"></param>
        /// <returns></returns>
        internal static DotNet.Models.CreateContainerResponse CreateContainer(
            string id,
            CreateContainerCmdlet cmdlet,
            DotNet.DockerClient dkrClient)
        {
            var configuration = cmdlet.Configuration;
            if (configuration == null)
            {
                configuration = new Config();
            }

            if (!String.IsNullOrEmpty(id))
            {
                configuration.Image = id;
            }

            if (cmdlet.Command != null)
            {
                configuration.Cmd = cmdlet.Command;
            }

            var hostConfiguration = cmdlet.HostConfiguration;
            if (hostConfiguration == null)
            {
                hostConfiguration = new HostConfig();
            }

            if (String.IsNullOrEmpty(hostConfiguration.Isolation))
            {
                hostConfiguration.Isolation = cmdlet.Isolation.ToString();
            }

            return dkrClient.Containers.CreateContainerAsync(
                new DotNet.Models.CreateContainerParameters()
                {
                    Name = cmdlet.ContainerName,
                    Cmd = configuration.Cmd,
                    Image = configuration.Image,
                    HostConfig = hostConfiguration
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
                dkrClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true }).AwaitResult(
                        ).Single(c => c.ID.StartsWith(id) || c.Names.Any(n => n.Equals("/" + id))),
                    dkrClient.Configuration.EndpointBaseUri.ToString());
        }

        /// <summary>
        /// Gets a single image object from the client by id.
        /// </summary>
        /// <param name="id">The image identifier to retrieve.</param>
        /// <param name="dkrClient">The client to request the image from.</param>
        /// <returns>The single image object matching the id.</returns>
        internal static Image GetImageById(string id, DotNet.DockerClient dkrClient)
        {
            var shaId = id;
            if (!shaId.StartsWith("sha256:"))
            {
                shaId = "sha256:" + shaId;
            }
            // TODO - Have a better way to get the image list response given the ID.
            return new Image(
                dkrClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }).AwaitResult(
                        ).Single(c => c.ID.StartsWith(shaId)),
                    dkrClient.Configuration.EndpointBaseUri.ToString());
        }
    }
}
