@echo off

IF /I "%1"=="-f" (del .\src\*project.lock.json /Q /S)

dotnet restore
dotnet build .\src\Docker.PowerShell -o .\artifacts\bin\Docker.PowerShell\Debug\net46 -f net46 -r win
