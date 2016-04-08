@echo off

IF /I "%1"=="-f" (del .\src\*project.lock.json /Q /S)

dotnet restore .\src\dotnet-tar\src\Tar\project.json
dotnet build .\src\dotnet-tar\src\Tar\project.json -o .\artifacts\bin\Tar\Debug\netstandard1.3 -f netstandard1.3
dotnet pack .\src\dotnet-tar\src\Tar\project.json -o .\artifacts\bin\Tar\Debug

dotnet restore .\src\Docker.DotNet\Docker.DotNet\project.json
dotnet build .\src\Docker.DotNet\Docker.DotNet\project.json -o .\artifacts\bin\Docker.DotNet\Debug\netstandard1.3 -f netstandard1.3
dotnet pack .\src\Docker.DotNet\Docker.DotNet\project.json -o .\artifacts\bin\Docker.DotNet\Debug

dotnet restore .\src\Docker.DotNet\Docker.DotNet.X509\project.json
dotnet build .\src\Docker.DotNet\Docker.DotNet.X509\project.json -o .\artifacts\bin\Docker.DotNet\Debug\net46 -f net46
dotnet pack .\src\Docker.DotNet\Docker.DotNet.X509\project.json -o .\artifacts\bin\Docker.DotNet\Debug

rmdir %USERPROFILE%\.nuget\packages\Docker.DotNet /Q /S
rmdir %USERPROFILE%\.nuget\packages\Docker.DotNet.X509 /Q /S

dotnet restore .\src\Docker.PowerShell\project.json
dotnet build .\src\Docker.PowerShell\project.json -o .\artifacts\bin\Docker.PowerShell\Debug\net46 -f net46 -r win
dotnet pack .\src\Docker.PowerShell\project.json -o .\artifacts\bin\Docker.PowerShell\Debug