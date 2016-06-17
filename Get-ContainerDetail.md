---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Get-ContainerDetail
## SYNOPSIS
Get-ContainerDetail \[-Id\] \<string\[\]\> \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Get-ContainerDetail \[-Container\] \<ContainerListResponse\[\]\> \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default
```
Get-ContainerDetail [-Id] <String[]> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### ContainerObject
```
Get-ContainerDetail [-Container] <ContainerListResponse[]> [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Gets details about a container.
## EXAMPLES

### Example 1
```
PS C:\> Get-ContainerDetail
```

Gets details about a container. 
## PARAMETERS

### -CertificateLocation
The location of the X509 certificate file named “key.pfx” that will be used for authentication with the server.  (Note that certificate authorization work is still in progress and this is likely to change).





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
Specifies the container to get details for.





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
Specifies the id of the container to get details for. This parameter also accepts container name, or a unique subset of the id.





```yaml
Type: String[]
Parameter Sets: Default
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String[]
Docker.DotNet.Models.ContainerListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ContainerInspectResponse

## NOTES

## RELATED LINKS

[Online Version:]()






