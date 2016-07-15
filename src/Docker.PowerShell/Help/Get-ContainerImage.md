---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Get-ContainerImage
## SYNOPSIS
Get-ContainerImage \[-All\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default (Default)
```
Get-ContainerImage [[-Id] <String[]>] [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### AllImages
```
Get-ContainerImage [-All] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Returns container images.
## EXAMPLES

### Example 1
```
PS C:\> Get-ContainerImage
```

Returns a list of container images. 
## PARAMETERS

### -All
Specifies that all images should be retrieved. 





```yaml
Type: SwitchParameter
Parameter Sets: AllImages
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -CertificateLocation
The location of the X509 certificate file named "key.pfx" that will be used for authentication with the server.  (Note that certificate authorization work is still in progress and this is likely to change).





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
Parameter Sets: Default
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
{{Fill Id Description}}

```yaml
Type: String[]
Parameter Sets: Default
Aliases: ImageName

Required: False
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### None

## OUTPUTS

### Docker.DotNet.Models.ImagesListResponse

## NOTES

## RELATED LINKS

[Online Version:]()






