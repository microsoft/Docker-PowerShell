---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Start-Container
## SYNOPSIS
Start-Container \[-Id\] \<string\[\]\> \[-PassThru\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Start-Container \[-Container\] \<ContainerListResponse\[\]\> \[-PassThru\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default (Default)
```
Start-Container [-PassThru] [-Id] <String[]> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### ContainerObject
```
Start-Container [-PassThru] [-Container] <ContainerListResponse[]> [-CertificateLocation <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Starts a container
## EXAMPLES

### Example 1
```
PS C:\> Start-Container $myContainer
```

Starts a container
## PARAMETERS

### -CertificateLocation
Cert location





```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Container
The container to start





```yaml
Type: ContainerListResponse[]
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -HostAddress
The address of the docker daemon to connect to.





```yaml
Type: String
Parameter Sets: Default
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Id of the container to start. Can be a unique subset of ID.





```yaml
Type: String[]
Parameter Sets: Default
Aliases: Name

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -PassThru
Passes the new container object through the pipeline.





```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
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

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/tree/master/src/Docker.PowerShell/en-us/)






