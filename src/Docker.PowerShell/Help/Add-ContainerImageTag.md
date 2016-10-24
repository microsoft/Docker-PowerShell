---
external help file: Docker.PowerShell.dll-Help.xml
online version: 
schema: 2.0.0
---

# Add-ContainerImageTag
## SYNOPSIS
Adds a repository and tag to the given image.
Aliased as "Tag-ContainerImage".
## SYNTAX

### Default (Default)
```
Add-ContainerImageTag [-Force] [-Repository] <String> [[-Tag] <String>] [-ImageIdOrName] <String[]>
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Add-ContainerImageTag [-Force] [-Repository] <String> [[-Tag] <String>] [-Image] <ImagesListResponse[]>
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Adds a repository and tag to the given image.
Aliased as "Tag-ContainerImage".
## EXAMPLES

### Example 1
```
PS C:\> Add-ContainerImageTag -ImageIdOrName 5b61 -Repository myImage -Tag 1.4
```

Add the repository/tag "myImage:1.4" to the image with id "5b61".
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

### -Force
Completes the operation without prompting for confirmation.

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

### -Image
The image the tag will be added to.

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

### -Repository
The new repository value that will be added to the image.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tag
The new tag value that will be added to the image.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImageIdOrName
The Id or Name of the image the tag will be added to.

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

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Add-ContainerImageTag.md)
