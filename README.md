#PowerShell for Docker
The goal of this project is to create a PowerShell module for the [Docker Engine]( https://github.com/docker/docker/), which can be utilized as an alternative or in conjunction with the Docker client (CLI).

##Requirements and Goals
### Requirements
1.	Must work on all versions of Windows Server including Nano Server.  This requires that it be Core PowerShell and CoreCLR compliant (see links below).
2.	Must follow the Docker Remote API interop and breaking change policy (https://docs.docker.com/engine/breaking_changes/)
3.	Must work locally or remotely, with remote following appropriate authentication and security mechanisms (i.e. TLS).

### Goals
* Follow the PowerShell naming and design guidelines (https://msdn.microsoft.com/en-us/library/ms714657(v=vs.85).aspx) including naming.
* Follow the general design and usage patterns of Docker

####.Net Core and Core PowerShell Resources
##### GitHub Repo’s for .Net Core CLR and .Net Core Libraries
* https://github.com/dotnet/coreclr
* https://github.com/dotnet/core

##### Good Resources Explaining .Net Core
* https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
* https://technet.microsoft.com/en-us/library/hh849695.aspx

##Installation


##High Level Design
The PowerShell module for Docker is built on top of the Docker Engine REST Interface (https://docs.docker.com/engine/reference/api/docker_remote_api/).
