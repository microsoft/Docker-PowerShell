@echo off

del .\src\dotnet-tar\src\Tar\project.lock.json
dotnet restore .\src\dotnet-tar\src\Tar\project.json
dotnet build .\src\dotnet-tar\src\Tar\project.json -o .\artifacts\bin\Tar\Debug\netstandard1.3 -f netstandard1.3
dotnet pack .\src\dotnet-tar\src\Tar\project.json -o .\artifacts\bin\Tar\Debug

del .\src\Docker.DotNet\Docker.DotNet\project.lock.json
dotnet restore .\src\Docker.DotNet\Docker.DotNet\project.json
dotnet build .\src\Docker.DotNet\Docker.DotNet\project.json -o .\artifacts\bin\Docker.DotNet\Debug\netstandard1.3 -f netstandard1.3
dotnet pack .\src\Docker.DotNet\Docker.DotNet\project.json -o .\artifacts\bin\Docker.DotNet\Debug

del .\src\Docker.PowerShell\project.lock.json
dotnet restore .\src\Docker.PowerShell\project.json
dotnet build .\src\Docker.PowerShell\project.json -o .\artifacts\bin\Docker.PowerShell\Debug\net46 -f net46 -r win
dotnet pack .\src\Docker.PowerShell\project.json -o .\artifacts\bin\Docker.PowerShell\Debug