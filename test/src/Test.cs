using System;
using System.Threading.Tasks;
using Docker.DotNet.Models;

using Xunit;

namespace Docker.PowerShell.Tests
{
    public class CertTests : IDisposable
    {
        [Fact]
        public async Task CertficatesShouldWork()
        {
            var client = DockerFactory.CreateClient(null, null);
            var listParams = new ImagesListParameters() { All = true };
            await client.Images.ListImagesAsync(listParams);
            //result.Wait();
        }

        public void Dispose(){}
    }
}
