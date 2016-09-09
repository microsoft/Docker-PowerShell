
$global:WindowsServerCore = "microsoft/windowsservercore"
$global:NanoServer = "microsoft/nanoserver"

$global:DefaultContainerImageName = $global:WindowsServerCore
$global:DefaultIsolatedContainerImageName = $global:NanoServer

function Test-Client
{
    $EditionId = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID').EditionId

    return $EditionId -notlike "*Server*"
}

function Test-Nano
{
    $EditionId = (Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID').EditionId

    return (($EditionId -eq "ServerStandardNano") -or 
            ($EditionId -eq "ServerDataCenterNano") -or 
            ($EditionId -eq "NanoServer") -or 
            ($EditionId -eq "ServerTuva"))
}

function Test-Server
{
    return -not(Test-Client) -and -not(Test-Nano)
}

function New-TempTestPath
{
    param(
        [string]
        $Context = [guid]::NewGuid().ToString()
    )

    $path = Join-Path "$env:TEMP" (Join-Path "Docker_CLI_Tests" $Context)
    New-Item -Path $path -ItemType "Container" | Out-Null
    return $path
}

function Test-ImportedModule
{
    param(
        [string]
        [ValidateNotNullOrEmpty()]
        $ModuleName
    )

    if (-not(Get-Module -Name "$ModuleName"))
    {
        Import-Module -Name "$ModuleName" -ErrorAction SilentlyContinue
    }
}

if (Test-Nano)
{
    $global:DefaultContainerImageName = $global:NanoServer
}