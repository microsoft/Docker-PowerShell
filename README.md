# PowerShell for Docker
This repo contains a PowerShell module for the [Docker Engine](
https://github.com/docker/docker/). It can be used as an alternative to the
Docker command-line interface (`docker`), or along side it. It can target a
Docker daemon running on any operating system that supports Docker, including
both Windows and Linux.

Note that this module is still in alpha status and is likely to change rapidly.

## Dependencies
* Windows or Windows Server (Nano Server coming soon)
* PowerShell 5 (available in Windows 10, Server 2016 Preview, or by installing
  [WMF 5](https://www.microsoft.com/en-us/download/details.aspx?id=50395))
* A recent Docker endpoint, running either locally or on a remote machine

Note that there is no dependency on the docker client.

Currently, the Docker endpoint must support API version 1.24, which is still in
development. Practically speaking, this means you will need a development build
of Docker. If your Docker endpoint is running Windows Server Technical Preview
5, that should be new enough.

## Installation
PowerShell for Docker is in prerelease, and there are no officially released
versions to try. However, you can try the development builds below and give us
feedback.

### Development Builds

![build status](https://ci.appveyor.com/api/projects/status/ysh1lrw8fxjjylpo/branch/master)

The following information will allow you to install development builds -- do
understand that these are early builds and will change (hopefully with your
feedback).

The dev builds are updated for every commit to master and are released to
https://ci.appveyor.com/nuget/docker-powershell-dev. To install the latest
build, in PowerShell run:

    > Register-PSRepository -Name DockerPS-Dev -SourceLocation https://ci.appveyor.com/nuget/docker-powershell-dev

    > Install-Module Docker -Repository DockerPS-Dev

After this, you can update to new development builds with just:

    > Update-Module Docker

## Contributions
We welcome contributions to this project in the form of issues (bugs,
suggestions, proposals, etc.) and pull requests.

For pull requests, we do require that you sign the [Microsoft Contribution
License Agreement](https://cla.microsoft.com/). It is a simple process that you
only need to complete once.

## Compilation
### Before Compiling: Git submodules for Docker.DotNet
This project uses Docker.DotNet as a git submodule (`git submodule --help` to
view manual pages for submodule).  When first starting a new clone of
Docker.Powershell, this requires one-time initializtion of the submodule with
`git submodule update --init` to prepare the directories used by the
submodule. When making changes to Docker.PowerShell that use corresponding
changes made to Docker.DotNet master branch, use `git submodule update --remote`
to sync the submodule to the latest in master, and include the updated commit id
for the submodule in the changes submitted to Docker.Powershell.

### Compiling with DotNet CLI
To compile this project, you need the following:
* A recent build of the [.NET Core SDK](https://github.com/dotnet/cli/releases)
* The .NET 4.6 and 4.5 SDKs. You get these either by:
  * Installing Visual Studio 2015 or
  * Downloading the [.NET 4.6 Targeting
Pack](https://www.microsoft.com/en-us/download/details.aspx?id=48136) 
and the [Windows SDK for Windows 8 with .NET Framework 4.5](https://developer.microsoft.com/en-us/windows/downloads/windows-8-sdk) 

Once these are installed, you can run:

    > dotnet restore

    > dotnet publish .\src\Docker.PowerShell -o .\src\Docker.PowerShell\bin\Module\Docker -r win

This will produce the PowerShell module at
`.\src\Docker.PowerShell\bin\Module\Docker` in the project folder.

You will only need to run `dotnet restore` once unless you pull changes that
update the project dependencies in project.json.

### Visual Studio Code
If you use [Visual Studio Code](https://code.visualstudio.com/) as your editor,
you will find three tasks pre-defined in the top-level directory:
* `restore` will perform `dotnet restore` across the whole project to reload any
  dependencies.
* `build` will perform the `dotnet publish` command with the arguments listed in
  the compilation section above.
* `test` will launch a new powershell window with the module loaded for manual
  testing.