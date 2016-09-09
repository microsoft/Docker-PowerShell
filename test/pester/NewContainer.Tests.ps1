<#
.DESCRIPTION
    This script will test the basic container creation.

#>

. .\Utils.ps1

function TestNewContainer
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

Describe "New-Container - Test matrix of types and hosts." {
    It "Create_WindowsServerCore" -Skip:$(Test-Client -or Test-Nano) {
        { TestNewContainer $global:WindowsServerCore $false } | Should Not Throw
    }

    It "Create_WindowsServerCore_Isolated" {
        { TestNewContainer $global:WindowsServerCore $true } | Should Not Throw
    }

    It "Create_NanoServer" -Skip:$(Test-Client) {
        { TestNewContainer $global:NanoServer $false } | Should Not Throw
    }

    It "Create_NanoServer_Isolated" {
        { TestNewContainer $global:NanoServer $true } | Should Not Throw
    }
}