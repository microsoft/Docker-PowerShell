using System;
using System.Linq;
using Docker.PowerShell.Cmdlets;

namespace Docker.PowerShell.Objects
{
    using System.Threading.Tasks;
    using DotNet.Models;

    public enum IsolationType
    {
        Default,
        None,
        HyperV
    }

    public class ContainerProcessExitException : Exception
    {
        public ContainerProcessExitException(int exitCode) : base(String.Format("Container process exited with non-zero exit code: {0}", exitCode)) { }
    }

    internal static class ContainerOperations
    {
        /// <summary>
        /// Creates the container
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmdlet"></param>
        /// <param name="dkrClient"></param>
        /// <returns></returns>
        internal static Task<CreateContainerResponse> CreateContainer(
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
                new CreateContainerParameters(configuration)
                {
                    Name = cmdlet.ContainerName,
                    HostConfig = hostConfiguration
                });
        }

        /// <summary>
        /// Gets a single container object from the client by id.
        /// </summary>
        /// <param name="id">The container identifier to retrieve.</param>
        /// <param name="dkrClient">The client to request the container from.</param>
        /// <returns>The single container object matching the id.</returns>
        internal static async Task<ContainerListResponse> GetContainerById(string id, DotNet.DockerClient dkrClient)
        {
            // TODO - Have a better way to get the container list response given the ID.
            return (await dkrClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true })).
                Single(c => c.ID.StartsWith(id) || c.Names.Any(n => n.Equals("/" + id)));
        }

        /// <summary>
        /// Gets a single image object from the client by id.
        /// </summary>
        /// <param name="id">The image identifier to retrieve.</param>
        /// <param name="dkrClient">The client to request the image from.</param>
        /// <returns>The single image object matching the id.</returns>
        internal static async Task<ImagesListResponse> GetImageById(string id, DotNet.DockerClient dkrClient)
        {
            var shaId = id;
            if (!shaId.StartsWith("sha256:"))
            {
                shaId = "sha256:" + shaId;
            }
            // TODO - Have a better way to get the image list response given the ID.
            return (await dkrClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }))
                .Single(c => c.ID.StartsWith(shaId));
        }

        /// <summary>
        /// Throws a ContainerProcessExitException if the given exit code is non-zero.
        /// </summary>
        /// <param name="exitCode">The process exit code.</param>
        internal static void ThrowOnProcessExitCode(long exitCode)
        {
            if (exitCode != 0)
            {
                throw new ContainerProcessExitException((int)exitCode);
            }
        }
    }
}
