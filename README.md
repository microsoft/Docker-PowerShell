# PowerShell for Docker
The goal of this project is to create a PowerShell module for the [Docker Engine]( https://github.com/docker/docker/), which can be utilized as an alternative or in conjunction with the Docker client (CLI).
## Current version
  0.1.0-alpha (pre-release)

## Requirements and Goals
### Requirements
1.	Must follow the Docker Remote API interop and breaking change policy (https://docs.docker.com/engine/breaking_changes/)
2.	Must work locally or remotely, with remote following appropriate authentication and security mechanisms (i.e. TLS).

### Goals
* Follow the PowerShell design guidelines (https://msdn.microsoft.com/en-us/library/ms714657(v=vs.85).aspx).
* Follow the general design and usage patterns of Docker
* Must remain CoreCLR compliant (see links below). Note that given the current requirements for the Docker.DotNet.X509, the project is still compiling for net46 instead of netstandard.  However, once updated implementation for TLS authentication is present, the project will be updated for netstandard and expected to compile and function on Nano Server using the .NET Core CLR and Core Libraries available there.


####.Net Core and Core PowerShell Resources
##### GitHub Repo’s for .Net Core CLR and .Net Core Libraries
* https://github.com/dotnet/coreclr
* https://github.com/dotnet/core

##### Good Resources Explaining .Net Core
* https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
* https://technet.microsoft.com/en-us/library/hh849695.aspx

## Compilation
To compile this project, use the dotnet CLI (https://github.com/dotnet/cli) to execute a restore command followed by a publish command:

`dotnet restore`

`dotnet publish .\\src\\Docker.PowerShell -o .\\src\\Docker.PowerShell\\bin\\Module\\Docker -r win`

This will produce the PowerShell module  at ".\src\Docker.PowerShell\bin\Module\Docker" in the project folder.  If Visual Studio is not already installed, the dotnet CLI may require an additional installation of the .NET 4.6 developer pack (https://www.microsoft.com/en-us/download/details.aspx?id=48136).

### Git submodules for Docker.DotNet
This project uses Docker.DotNet as a git submodule (`git submodule --help` to view manual pages for submodule).  When first starting a new clone of Docker.Powershell, this requires one-time initializtion of the submodule with `git submodule update --init --remote` to prepare the directories used by the submodule. When making changes to Docker.PowerShell that use corresponding changes made to Docker.DotNet master branch, use `git submodule update --remote` to sync the submodule to the latest in master, and include the updated commit id for the submodule in the changes submitted to Docker.Powershell.

## Using Visual Studio Code
This project also includes support for Visual Studio Code (https://code.visualstudio.com/, https://github.com/Microsoft/vscode) in the .vscode folder.  Using VSCode on the top-level folder, the restore, build, and test tasks will be available.  Restore will perform `dotnet restore` across the whole project to reload any dependencies. Build will perform the `dotnet publish` command with the arguments listed in the compilation section above.  Test will launch a new powershell window with the module loaded for manual testing.

## High Level Design
The PowerShell module for Docker is built on top of the Docker Engine REST Interface (https://docs.docker.com/engine/reference/api/docker_remote_api/).
