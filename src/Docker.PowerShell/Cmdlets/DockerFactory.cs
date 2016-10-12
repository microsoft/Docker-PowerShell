using System;
using System.Collections;
using Docker.DotNet;
using Docker.DotNet.X509;

#if !NET46
using System.Runtime.InteropServices;
#else
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Docker.PowerShell.Support;
#endif

public class DockerFactory
{
    private const string ApiVersion = "1.24";
    private const string CertFileName = "cert.pem";
    private const string CertKeyFileName = "key.pem";
    private const string CAFileName = "ca.pem";

    private static readonly bool IsWindows =
#if NET46
                true;
#else
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

    private static readonly string DefaultHost = IsWindows ? "npipe://./pipe/docker_engine" : "unix://var/run/docker.sock";

    public static DockerClient CreateClient(string HostAddress, string CertificateLocation)
    {
        HostAddress = HostAddress ?? Environment.GetEnvironmentVariable("DOCKER_HOST") ?? DefaultHost;

        CertificateLocation = CertificateLocation ?? Environment.GetEnvironmentVariable("DOCKER_CERT_PATH");

        CertificateCredentials cred = null;
        if (!string.IsNullOrEmpty(CertificateLocation))
        {
#if !NET46
                throw new InvalidOperationException("TLS authetication is not supported in .NET Core.");
#else
            // Try to find a certificate for secure connections.
            var clientCert = new X509Certificate2(System.IO.Path.Combine(CertificateLocation, CertFileName));
            clientCert.PrivateKey = PrivateKey.ReadFromPemFile(System.IO.Path.Combine(CertificateLocation, CertKeyFileName));
            cred = new CertificateCredentials(clientCert);

            var tlsVerify = Environment.GetEnvironmentVariable("DOCKER_TLS_VERIFY") == "1";
            X509Certificate2 caCert = null;
            if (tlsVerify)
            {
                caCert = new X509Certificate2(System.IO.Path.Combine(CertificateLocation, CAFileName));
            }

            cred.ServerCertificateValidationCallback = (o, c, ch, er) =>
            {
                if (er == SslPolicyErrors.None || !tlsVerify)
                {
                    return true;
                }

                try
                {
                    using (var chain = new X509Chain())
                    {
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                        chain.ChainPolicy.ExtraStore.Add(caCert);

                        if (!chain.Build(new X509Certificate2(c)))
                        {
                            throw new InvalidOperationException("Failed to build certificate chain for server certificate.");
                        }

                        return chain.ChainStatus.Length == 0 || 
                            (chain.ChainStatus.Length == 1 && chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot);
                    }
                }
                catch{ return true;}
            };
#endif
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