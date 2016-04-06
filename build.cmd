@echo off

dotnet restore
dotnet build .\src\Docker.PowerShell\project.json -o .\artifacts\bin -f net46