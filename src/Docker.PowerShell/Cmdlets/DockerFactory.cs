using System;
using System.Collections;
using Docker.DotNet;
using Docker.DotNet.X509;

#if !NET46
using System.Runtime.InteropServices;
#else
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
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
            cred = new CertificateCredentials(
                RSAUtil.GetCertFromPEMFiles(
                    System.IO.Path.Combine(CertificateLocation, CertFileName),
                    System.IO.Path.Combine(CertificateLocation, CertKeyFileName)));

            var tlsVerify = Environment.GetEnvironmentVariable("DOCKER_TLS_VERIFY") == "1";
            X509Certificate2 caCert = null;
            if (tlsVerify)
            {
                caCert = new X509Certificate2(System.IO.Path.Combine(CertificateLocation, CAFileName));
            }

            cred.ServerCertificateValidationCallback = (obj, cert, chain, error) =>
            {
                if (error == SslPolicyErrors.None || !tlsVerify)
                {
                    return true;
                }

                using (var chain2 = new X509Chain())
                {
                    chain2.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    chain2.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                    chain2.ChainPolicy.ExtraStore.Add(caCert);

                    if (!chain2.Build(new X509Certificate2(cert)))
                    {
                        throw new InvalidOperationException("Failed to build certificate chain for server certificate.");
                    }

                    return chain2.ChainStatus.Length == 0 || 
                        (chain2.ChainStatus.Length == 1 && chain2.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot);
                }
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