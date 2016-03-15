using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;

namespace Docker.PowerShell.Objects
{
    public class Container : DotNet.Models.ContainerListResponse
    {
        public string HostAddress;

        public Container(DotNet.Models.ContainerListResponse c, string hostAddress)
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
        }
    }

    public class Image : DotNet.Models.ImageListResponse
    {
        public string HostAddress;

        public Image(DotNet.Models.ImageListResponse i, string hostAddress)
        {
            Id = i.Id;
            ParentId = i.ParentId;
            RepoTags = i.RepoTags;
            Created = i.Created;
            Size = i.Size;
            VirtualSize = i.VirtualSize;
            HostAddress = hostAddress;
        }
    }
}
