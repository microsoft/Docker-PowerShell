<#
.DESCRIPTION
    This script will test the basic container creation.

#>

. .\Utils.ps1

function TestStartContainer
{
    param(
        [string]
        [ValidateNotNullOrEmpty()]
        $ImageName,

        [bool]
        $IsIsolated
    )

    # We need to module imported so we can create the types before invocation.
    Test-ImportedModule "Docker"

    $isolation = [Docker.PowerShell.Objects.IsolationType]::Default
    if ($IsIsolated)
    {
        $isolation = [Docker.PowerShell.Objects.IsolationType]::HyperV
    }

    try 
    {
        $container = New-Container -Id "$ImageName" -Isolation $isolation -Command @("cmd", "/c", "echo Worked")
        $container | Should Not Be $null

        $container | Start-Container

        $container | Wait-Container
    }
    finally
    {
        # Cleanup
        if ($container)
        {
            $container | Remove-Container
        }
    }
}

Describe "Start-Container - Test matrix of types and hosts." {
    It "Start_WindowsServerCore" -Skip:$(Test-Client -or Test-Nano) {
        { TestStartContainer $global:WindowsServerCore $false } | Should Not Throw
    }

    It "Start_WindowsServerCore_Isolated" {
        { TestStartContainer $global:WindowsServerCore $true } | Should Not Throw
    }

    It "Start_NanoServer" -Skip:$(Test-Client) {
        { TestStartContainer $global:NanoServer $false } | Should Not Throw
    }

    It "Start_NanoServer_Isolated" {
        { TestStartContainer $global:NanoServer $true } | Should Not Throw
    }
}