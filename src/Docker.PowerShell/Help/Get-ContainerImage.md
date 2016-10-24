---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Get-ContainerImage
## SYNOPSIS
Returns container images.
## SYNTAX

### Default (Default)
```
Get-ContainerImage [[-ImageIdOrName] <String[]>] [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### AllImages
```
Get-ContainerImage [-All] [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
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
Specifies that all images should be retrieved. Otherwise, intermediate images are suppressed.





```yaml
Type: SwitchParameter
Parameter Sets: AllImages
Aliases: 

Required: False
Position: Named
Default value: None
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

### -ImageIdOrName
An array of names or ids of the images to be retrieved

```yaml
Type: String[]
Parameter Sets: Default
Aliases: ImageName, ImageId

Required: False
Position: 0
Default value: None
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

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Get-ContainerImage.md)






