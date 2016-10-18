---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# New-ContainerImage
## SYNOPSIS
Builds a new container image from a set of instructions in a Dockerfile.
Aliased as "Build-ContainerImage".
## SYNTAX

```
New-ContainerImage [[-Path] <String>] [-Repository <String>] [-Tag <String>] [-Isolation <IsolationType>]
 [-SkipCache] [-ForceRemoveIntermediateContainers] [-PreserveIntermediateContainers] [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Builds a new container image from a set of instructions in a Dockerfile.
Aliased as "Build-ContainerImage".
## EXAMPLES

### Example 1
```
PS C:\> New-ContainerImage
```

Creates a new container image from a Dockerfile in the current folder, "C:\".
### Example 2
```
PS C:\> New-ContainerImage -Path "C:\myData\"
```

Creates a new container image from a Dockerfile in the specified folder, "C:\myData".
 
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

### -ForceRemoveIntermediateContainers
Indicates that containers used for intermediate steps during build should always be deleted, even if that build step failed.





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

### -Path
The location to the folder containing the Dockerfile or a path to a file to use as the Dockerfile. If not specified, the current working directory is used.





```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PreserveIntermediateContainers
Indicates that the containers used for intermediate steps during build should not be automatically deleted.





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

### -Repository
Specifies the repository for the new image.





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

### -SkipCache
Forces a rebuild of the image without using any of the layers on disk.





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

### -Tag
Sets a tag for the new image. If no tag is specified, the image will be tagged "latest".





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

### -Isolation
The isolation level used for containers during the build.

```yaml
Type: IsolationType
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/New-ContainerImage.md)






