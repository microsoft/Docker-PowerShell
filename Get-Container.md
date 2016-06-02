---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Get-Container
## SYNOPSIS
Get-Container \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

```
Get-Container [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Retuns a list of containers.
## EXAMPLES

### Example 1
```
PS C:\> Get-Container
```

Retrieves a list of containers. 
## PARAMETERS

### -CertificateLocation
Specifies certificate location.





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

### -HostAddress
Specifies Host Address.





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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### None

## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES
These are notes about the cmdlet. 

## RELATED LINKS

[Online Version:](https://github.com/aoatkinson/Docker-PowerShell/blob/master/src/Docker.PowerShell/Docker.PowerShell.dll-help.xml)






