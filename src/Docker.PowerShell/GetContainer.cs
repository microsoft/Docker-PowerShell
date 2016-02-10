using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Management.Automation;
using Docker.DotNet;

namespace Docker.PowerShell
{
    [Cmdlet("Get", "Container")]
    public class GetContainer : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            var c = new DockerClientConfiguration(new Uri("http://10.121.234.135:2375")).CreateClient();
            var cs = c.Containers.ListContainersAsync(new Docker.DotNet.Models.ListContainersParameters() { Limit = 10 }).Result;
            foreach (var cont in cs)
            {
                Console.WriteLine(cont.Id);
            }
        }
    }
}
