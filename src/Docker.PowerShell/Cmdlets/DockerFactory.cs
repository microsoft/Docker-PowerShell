using System;
using System.Collections;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Docker.DotNet;
using Docker.DotNet.X509;

public class DockerFactory
{
    private const string ApiVersion = "1.24";
    private const string KeyFileName = "key.pfx";
    private const string certPass = "p@ssw0rd";

    public static DockerClient CreateClient(string HostAddress, string CertificateLocation)
    {
        HostAddress = HostAddress ?? Environment.GetEnvironmentVariable("DOCKER_HOST") ?? "http://127.0.0.1:2375";
        CertificateLocation = CertificateLocation ?? Environment.GetEnvironmentVariable("DOCKER_CERT_PATH");

        Credentials cred = null;
        if (!string.IsNullOrEmpty(CertificateLocation))
        {
            //BUGBUG(swernli) - Remove this later in favor of something better.
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

            // Try to find a certificate for secure connections.
            cred = new CertificateCredentials(
                    new X509Certificate2(
                        System.IO.Path.Combine(CertificateLocation, KeyFileName),
                        certPass));
        }

        return new DockerClientConfiguration(new Uri(HostAddress), cred).CreateClient(new Version(ApiVersion));
    }

    public static DockerClient CreateClient(IDictionary fakeBoundParameters)
    {
        var hostAddress = fakeBoundParameters["HostAddress"] as string;
        var certificateLocation = fakeBoundParameters["CertificateLocation"] as string;
        return DockerFactory.CreateClient(hostAddress, certificateLocation);
    }
}