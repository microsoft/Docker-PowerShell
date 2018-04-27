# THIS MODULE HAS BEEN DEPRECATED

> Please note that due to low usage this module is no longer being actively maintained. It is recommended to use either the `Docker cli (docker.exe)` or try [Docker.DotNet](https://github.com/Microsoft/Docker.DotNet) directly.

---

# PowerShell for Docker

This repo contains a PowerShell module for the [Docker Engine](
https://github.com/docker/docker/). It can be used as an alternative to the
Docker command-line interface (`docker`), or along side it. It can target a
Docker daemon running on any operating system that supports Docker, including
both Windows and Linux.

Note that this module is still in alpha status and is likely to change rapidly.

## Dependencies
* Windows, Windows Server, Nano Server, or PowerShell 6 supported host.
* PowerShell 5 (available in Windows 10, Server 2016 Preview, or by installing
  [WMF 5](https://www.microsoft.com/en-us/download/details.aspx?id=50395)) or
  [PowerShell 6 Preview](https://github.com/powershell/powershell) (available for Windows, Linux, and Mac OS X)
* A recent Docker endpoint, running either locally or on a remote machine.
  [Container Quick Start for Windows Server](https://msdn.microsoft.com/en-us/virtualization/windowscontainers/quick_start/quick_start_windows_server)
  [Container Quick Start for Windows 10](https://msdn.microsoft.com/en-us/virtualization/windowscontainers/quick_start/quick_start_windows_10)

Note that there is no dependency on the Docker client.

Currently, the Docker endpoint must support API version 1.24, which is still in
development. Practically speaking, this means you will need a development build
of Docker. If your Docker endpoint is running on Windows Server Technical Preview
5, that should be new enough.

## Installation
See our [Releases](https://github.com/Microsoft/Docker-PowerShell/releases) page for current releases
of Docker PowerShell, or you can try the development builds below. Feedback and
contributions welcome!

### Development Builds

![build status](https://ci.appveyor.com/api/projects/status/ysh1lrw8fxjjylpo/branch/master)

The following information will allow you to install development builds -- do
understand that these are early builds and will change (hopefully with your
feedback).

The dev builds are updated for every commit to master and are released to
https://ci.appveyor.com/nuget/docker-powershell-dev. To install the latest
build, in PowerShell 5.0 run:

    > Register-PSRepository -Name DockerPS-Dev -SourceLocation https://ci.appveyor.com/nuget/docker-powershell-dev

    > Install-Module -Name Docker -Repository DockerPS-Dev -Scope CurrentUser
(Note: if you get an error like "WARNING: MSG:SourceLocationNotValid" try updating your Nu-Get version as explained in [this issue comment](https://github.com/Microsoft/Docker-PowerShell/issues/62#issuecomment-221659534).)

After this, you can update to new development builds with just:

    > Update-Module -Name Docker

#### Linux and Mac OS X

As of the [v6.0.0-alpha.10](https://github.com/PowerShell/PowerShell/releases/tag/v6.0.0-alpha.10) release of PowerShell, the instructions
listed above for development builds work as expected on Linux.

#### Need an offline workflow?

From an internet connected machine:

    > Save-Module -Name Docker -Path .

Copy the entire folder `.\Docker` to the destination at: `%ProgramFiles%\WindowsPowerShell\Modules`

## Contributions
We welcome contributions to this project in the form of issues (bugs,
suggestions, proposals, etc.) and pull requests.

For pull requests, we do require that you sign the [Microsoft Contribution
License Agreement](https://cla.microsoft.com/). It is a simple process that you
only need to complete once.

This project has adopted the [Microsoft Open Source Code of
Conduct](https://opensource.microsoft.com/codeofconduct/). For more information
see the [Code of Conduct
FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact
[opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional
questions or comments.

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

    > dotnet publish .\src\Docker.PowerShell -o .\src\Docker.PowerShell\bin\Module\Docker -f net46

or for a module targetted at cross-platform PowerShell:

    > dotnet restore

    > dotnet publish .\src\Docker.PowerShell -o .\src\Docker.PowerShell\bin\Module\Docker -f netstandard1.6

This will produce the PowerShell module at
`.\src\Docker.PowerShell\bin\Module\Docker` in the project folder.

You will only need to run `dotnet restore` once unless you pull changes that
update the project dependencies in project.json.

### Updating Help Markdown Files
This codebase includes markdown files corresponding to the help information for
each of the cmdlets. This markdown is generated and consumed by the
[platyPS PowerShell module](https://github.com/PowerShell/platyPS). You should use
the platyPS cmdlets to update and refresh the markdown files to reflect any changes
made to the structure or behavior of the cmdlets.  Follow the instructions on the
platyPS github readme to get the module installed, and then after imported the
Docker module with your changes compiled in, run:

    > New-MarkdownHelp -Module Docker -OutputFolder .\src\Docker.Powershell\Help -ErrorAction SilentlyContinue

    > Update-MarkdownHelp -Path .\src\Docker.PowerShell\Help

This will create new entries for any added parameters in existing cmdlets, as well as
new markdown files for any new cmdlets, leaving placeholder text for the descriptions
and examples. Please keep the descriptions of any existing or new parameters or cmdlets
up-to-date with any changes to the implementation.

### Visual Studio Code
If you use [Visual Studio Code](https://code.visualstudio.com/) as your editor,
you will find three tasks pre-defined in the top-level directory:
* `restore` will perform `dotnet restore` across the whole project to reload any
  dependencies.
* `build` will perform the `dotnet publish` command with the arguments listed in
  the compilation section above.
* `test` will launch a new powershell window with the module loaded for manual
  testing.
* `update-help` will use powershell to run the New-MarkdownHelp and
  Update-MarkdownHelp cmdlets with the arguments necessary to generate any
  updates to the help markdown files. NOTE: This requires the platyPS module
  and uses the most recently built local Docker module.
