<#
.DESCRIPTION
    This script will test the basic build functionality.

#>

. .\Utils.ps1

function CreateDockerfile
{
    param(
        [string]
        [ValidateNotNullOrEmpty()]
        $BasePath,

        [string]
        [ValidateNotNullOrEmpty()]
        $ImageName
    )

    $filepath = Join-Path $BasePath "Dockerfile"
    Write-Host "Creating dockerfile at path: '$filepath'."

    "FROM $ImageName" | Out-File -FilePath $filePath -Encoding utf8 -Append 
    "RUN echo test > test.txt" | Out-File -FilePath $filePath -Encoding utf8 -Append

    Write-Host "Successfully created dockerfile."
}

function TestImageBuild
{
    param(
        [string]
        [ValidateNotNullOrEmpty()]
        $ImageName,

        [bool]
        $IsIsolated,

        [string]
        [ValidateNotNullOrEmpty()]
        $Tag
    )

	# We need to module imported so we can create the types before invocation.
    Test-ImportedModule "Docker"
	
    $basePath = New-TempTestPath
    CreateDockerfile $basePath $ImageName

    $isolation = [Docker.PowerShell.Objects.IsolationType]::Default
    if ($IsIsolated)
    {
        $isolation = [Docker.PowerShell.Objects.IsolationType]::HyperV
    }
    
    Write-Host "Building image: '$Tag'"
    Build-ContainerImage -Path "$basePath" -Repository "$Tag" -SkipCache -Isolation $isolation
}

function TestImageBuilds
{
    param(
        [string]
        [ValidateNotNullOrEmpty()]
        $ImageName,

        [bool]
        $IsIsolated
    )

    $tag = "test"
    if ($IsIsolated)
    {
        $tag = "isolated" + $tag
    }

    $firstTag = $tag + "1"
    $secondTag = $tag + "2"

    try
    {
        # Test a level 1 build.
        $image = TestImageBuild "$ImageName" $IsIsolated "$firstTag"
        $image | Should Not Be $null

        # Test a second build based on the first.
        $image2 = TestImageBuild "$firstTag" $IsIsolated "$secondTag"
        $image2 | Should Not Be $null
    }
    finally
    {
        # Cleanup
        if ($image2)
        {
            $image2 | Remove-ContainerImage
        }

        if ($image)
        {
            $image | Remove-ContainerImage
        }
    }
}

Describe "Build-ContainerImage - Test matrix of types and hosts." {
    It "WindowsServerCore_Image_Build" -Skip:$(Test-Client -or Test-Nano) {
        { TestImageBuilds $global:WindowsServerCore $false } | Should Not Throw
    }

    It "WindowsServerCore_Isolated_Image_Build" {
        { TestImageBuilds $global:WindowsServerCore $true } | Should Not Throw
    }

    It "NanoServer_Image_Build" -Skip:$(Test-Client) {
        { TestImageBuilds $global:NanoServer $false } | Should Not Throw
    }

    It "NanoServer_Isolated_Image_Build" {
        { TestImageBuilds $global:NanoServer $true } | Should Not Throw
    }
}