using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;

namespace Docker.PowerShell.Objects
{
    public enum IsolationType
    {
        Default,
        None,
        HyperV
    }

    public class Container : DotNet.Models.ContainerListResponse
    {
        public string HostAddress;

        public Container(DotNet.Models.ContainerListResponse c, string hostAddress)
        {
            ID = c.ID;
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

    public class Image : DotNet.Models.ImagesListResponse
    {
        public string HostAddress;

        public Image(DotNet.Models.ImagesListResponse i, string hostAddress)
        {
            ID = i.ID;
            ParentID = i.ParentID;
            RepoTags = i.RepoTags;
            Created = i.Created;
            Size = i.Size;
            VirtualSize = i.VirtualSize;
            HostAddress = hostAddress;
        }
    }

    public class Config : DotNet.Models.Config
    {
        public Config() { }
    }

    public class HostConfig : DotNet.Models.HostConfig
    {
        public HostConfig() { }
    }
}
