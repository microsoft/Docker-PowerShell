using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;

namespace Docker.PowerShell.Objects
{
    public class ContainerListResponse : DotNet.Models.ContainerListResponse
    {
        public string HostAddress;
        public string ApiVersion;

        public ContainerListResponse(DotNet.Models.ContainerListResponse c, string hostAddress, string apiVersion)
        {
            Id = c.Id;
            Names = c.Names;
            Image = c.Image;
            Created = c.Created;
            Command = c.Command;
            Status = c.Status;
            SizeRw = c.SizeRw;
            SizeRootFs = c.SizeRootFs;
            Ports = c.Ports;
            HostAddress = hostAddress;
            ApiVersion = apiVersion;
        }
    }

    public class ImageListResponse : DotNet.Models.ImageListResponse
    {
        public string HostAddress;
        public string ApiVersion;

        public ImageListResponse(DotNet.Models.ImageListResponse i, string hostAddress, string apiVersion)
        {
            Id = i.Id;
            ParentId = i.ParentId;
            RepoTags = i.RepoTags;
            Created = i.Created;
            Size = i.Size;
            VirtualSize = i.VirtualSize;
            HostAddress = hostAddress;
            ApiVersion = apiVersion;
        }
    }
}
