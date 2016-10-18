---
external help file: Docker.PowerShell.dll-Help.xml
online version: https://github.com/Microsoft/Docker-PowerShell
schema: 2.0.0
---

# Export-ContainerImage
## SYNOPSIS
Exports the container image, including all layers, into a single compressed file.
Aliased to "Save-ContainerImage".
## SYNTAX

### Default (Default)
```
Export-ContainerImage -DestinationFilePath <String> [-ImageIdOrName] <String[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Export-ContainerImage -DestinationFilePath <String> [-Image] <ImagesListResponse[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Exports the container image, including all layers, into a single compressed file.
Aliased to "Save-ContainerImage".
## EXAMPLES

### Example 1
```
PS C:\> Export-ContainerImage -ImageIdOrName myImage -DestinationFilePath c:\myImage.tar
```

Exports all layers for the image "myImage" to the compressed file "c:\myImage.tar".
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

### -Image
The image that will be exported to a file.

```yaml
Type: ImagesListResponse[]
Parameter Sets: ImageObject
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DestinationFilePath
The path to export the image to.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImageIdOrName
The Name or Id of the image that will be exported.

```yaml
Type: String[]
Parameter Sets: Default
Aliases: ImageName, ImageId

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
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Export-ContainerImage.md)
