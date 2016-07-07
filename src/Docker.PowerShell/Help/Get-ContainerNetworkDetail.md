---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Get-ContainerNetworkDetail
## SYNOPSIS
Get-ContainerNetworkDetail \[-Name\] \<string\> \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

```
Get-ContainerNetworkDetail [-Name] <String> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Gets details about a container's network.
## EXAMPLES

### Example 1
```
PS C:\> Get-ContainerNetworkDetail -Name mycontainer
```

Gets network details for the container "mycontainer"
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

### -HostAddress
The address of the docker daemon to connect to.





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

### -Name
Specifies the name of the container to get network details for. 





```yaml
Type: String
Parameter Sets: (All)
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

### System.String

## OUTPUTS

### Docker.DotNet.Models.NetworkResponse

## NOTES

## RELATED LINKS

[Online Version:]()






