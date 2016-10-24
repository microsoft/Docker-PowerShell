---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Start-Container
## SYNOPSIS
Starts a container.
## SYNTAX

### Default (Default)
```
Start-Container [-Attach] [-Input] [-PassThru] [-ContainerIdOrName] <String[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
Start-Container [-Attach] [-Input] [-PassThru] [-Container] <ContainerListResponse[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Starts a container.
## EXAMPLES

### Example 1
```
PS C:\> Start-Container $myContainer
```

Starts a container.
## PARAMETERS

### -CertificateLocation
The location of the X509 certificate file named "key.pfx" that will be used for authentication with the server. (Note that certificate authorization work is still in progress and this is likely to change).





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

### -Container
The container to start.





```yaml
Type: ContainerListResponse[]
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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

### -PassThru
Passes the started container object through the pipeline.





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

### -Attach
If set attaches STDOUT, STDERR and reports to the console. 

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
Indicates that the stdin of the container should be kept open.

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
The Id or Name of the container to start.

```yaml
Type: String[]
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
Docker.DotNet.Models.ContainerListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Start-Container.md)






