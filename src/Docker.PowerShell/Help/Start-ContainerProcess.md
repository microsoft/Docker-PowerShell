---
external help file: Docker.PowerShell.dll-Help.xml
online version: https://github.com/Microsoft/Docker-PowerShell/tree/master/src/Docker.PowerShell/en-us/
schema: 2.0.0
---

# Start-ContainerProcess
## SYNOPSIS
Starts a new process with the given command in the specified container.
Aliased as "Exec-Container".
## SYNTAX

### Default (Default)
```
Start-ContainerProcess [[-Command] <String[]>] [-Detached] [-Input] [-Terminal] [-Privileged] [-User <String>]
 [-ContainerIdOrName] <String> [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
Start-ContainerProcess [[-Command] <String[]>] [-Detached] [-Input] [-Terminal] [-Privileged] [-User <String>]
 [-Container] <ContainerListResponse> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Starts a new process with the given command in the specified container.
Aliased as "Exec-Container".
## EXAMPLES

### Example 1
```
PS C:\> Start-ContainerProcess -ContainerIdOrName myContainer -Command "c:\myTest.exe"
```

Starts the command "c:\myTest.exe" in the container named "myContainer".
## PARAMETERS

### -CertificateLocation
The location of the X509 certificate file named "key.pfx" that will be used for authentication with the server.  (Note that certificate authorization work is still in progress and this is likely to change).

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Command
The command to be run as a new process in the container. 

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Container
The container in which the process will be started.

```yaml
Type: ContainerListResponse
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Detached
If specified, just the container process object will be returned and the process will run asynchronously. Otherwise, STDOUT/STDERR will be connected and output of the command will be displayed.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -HostAddress
The address of the docker daemon to connect to.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Privileged
If specified, the process will be started in privileged mode.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -User
A custom user name under which the process will be created.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Terminal
If specified, terminal emulation will be used when starting the process.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Input
Indicates that the STDIN of the process should be kept open.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContainerIdOrName
The Id or Name of the container to start the command in.

```yaml
Type: String
Parameter Sets: Default
Aliases: Name, Id

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String[]
System.String
Docker.DotNet.Models.ContainerListResponse
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Start-ContainerProcess.md)
