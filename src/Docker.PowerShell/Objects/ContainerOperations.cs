using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.PowerShell.Cmdlets;
using Docker.PowerShell.Support;

namespace Docker.PowerShell.Objects
{
    public enum IsolationType
    {
        Default,
        Process,
        HyperV
    }

    public class ContainerProcessExitException : Exception
    {
        public ContainerProcessExitException(long exitCode) : base(string.Format("Container process exited with non-zero exit code: {0}", exitCode)) { }
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
            DockerClient dkrClient)
        {
            var configuration = cmdlet.Configuration ?? new Config();

            if (!string.IsNullOrEmpty(id))
            {
                configuration.Image = id;
            }

            if (cmdlet.Command != null)
            {
                configuration.Cmd = cmdlet.Command;
            }

            var hostConfiguration = cmdlet.HostConfiguration ?? new HostConfig();

            if (string.IsNullOrEmpty(hostConfiguration.Isolation))
            {
                hostConfiguration.Isolation = cmdlet.Isolation.ToString();
            }

            configuration.Tty = cmdlet.Terminal;
            configuration.OpenStdin = cmdlet.Input;
            configuration.AttachStdin = cmdlet.Input;
            configuration.AttachStdout = true;
            configuration.AttachStderr = true;

            return dkrClient.Containers.CreateContainerAsync(
                new CreateContainerParameters(configuration)
                {
                    Name = cmdlet.Name,
                    HostConfig = hostConfiguration
                });
        }
        
        internal static Task<IList<ContainerListResponse>> GetContainersById(string id, DockerClient dkrClient)
        {
            return (dkrClient.Containers.ListContainersAsync(new ContainersListParameters
                    { 
                        All = true, 
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

        internal static Task<IList<ContainerListResponse>> GetContainersByName(string name, DockerClient dkrClient)
        {
            return (dkrClient.Containers.ListContainersAsync(new ContainersListParameters
                    { 
                        All = true, 
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

        /// <summary>
        /// Gets a single container object from the client by id or name.
        /// </summary>
        /// <param name="id">The container identifier to retrieve.</param>
        /// <param name="dkrClient">The client to request the container from.</param>
        /// <returns>The single container object matching the id.</returns>
        internal static async Task<IList<ContainerListResponse>> GetContainersByIdOrName(string id, DockerClient dkrClient)
        {
            return (await GetContainersByName(id, dkrClient)).Where(c => c.Names.Contains($"/{id}")).Concat(await GetContainersById(id, dkrClient)).ToList();
        }

        /// <summary>
        /// Gets a single image object from the client by id.
        /// </summary>
        /// <param name="id">The image identifier to retrieve.</param>
        /// <param name="dkrClient">The client to request the image from.</param>
        /// <returns>The single image object matching the id.</returns>
        internal static async Task<ImagesListResponse> GetImageById(string id, DockerClient dkrClient)
        {
            var shaId = id;
            if (!shaId.StartsWith("sha256:"))
            {
                shaId = "sha256:" + shaId;
            }
            // TODO - Have a better way to get the image list response given the ID.
            return (await dkrClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }))
                .Single(i => i.ID.StartsWith(shaId));
        }

        /// <summary>
        /// Gets any image objects from the client by matching repository:tag.
        /// </summary>
        /// <param name="repoTag">The image repository:tag to look for.</param>
        /// <param name="dkrClient">The client to request the image from.</param>
        /// <returns>The image objects matching the repository:tag.</returns>
        internal static async Task<IList<ImagesListResponse>> GetImagesByRepoTag(string repoTag, DockerClient dkrClient)
        {
            return (await dkrClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }))
                .Where(i => i.RepoTags.Any(rt => repoTag.Split('/').Last().Contains(":") ? rt == repoTag : rt == (repoTag + ":latest"))).ToList();
        }

        /// <summary>
        /// Throws a ContainerProcessExitException if the given exit code is non-zero.
        /// </summary>
        /// <param name="exitCode">The process exit code.</param>
        internal static void ThrowOnProcessExitCode(long exitCode)
        {
            if (exitCode != 0)
            {
                throw new ContainerProcessExitException(exitCode);
            }
        }

        internal static async Task StartContainerAsync(
            DockerClient client,
            string containerId,
            ContainerAttachParameters attachParams,
            bool? isTTY,
            ContainerStartParameters startParams,
            CancellationToken token)
        {
            MultiplexedStream stream = null;
            Task streamTask = null;

            try
            {
                if (attachParams != null)
                {
                    stream = await client.Containers.AttachContainerAsync(containerId, isTTY.GetValueOrDefault(), attachParams, token);
                    streamTask = stream.CopyToConsoleAsync(isTTY.GetValueOrDefault(), attachParams.Stdin.GetValueOrDefault(), token);
                }

                if (!await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters()))
                {
                    throw new ApplicationFailedException("The container has already started.");
                }

                if (attachParams != null)
                {
                    await streamTask;
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }
}
