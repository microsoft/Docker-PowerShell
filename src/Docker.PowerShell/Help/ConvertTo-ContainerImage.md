---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# ConvertTo-ContainerImage
## SYNOPSIS
Creates a new container image by committing an existing container.
Aliased as "Commit-Container".
## SYNTAX

### Default (Default)
```
ConvertTo-ContainerImage [-Repository <String>] [-Tag <String>] [-Message <String>] [-Author <String>]
 [-Configuration <Config>] [-ContainerIdOrName] <String[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
ConvertTo-ContainerImage [-Repository <String>] [-Tag <String>] [-Message <String>] [-Author <String>]
 [-Configuration <Config>] [-Container] <ContainerListResponse[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Creates a new container image by committing an existing container.
Aliased as "Commit-Container".
## EXAMPLES

### Example 1
```
PS C:\> New-Container -Name myContainer -Image $myImage -Command myWorkload.exe
PS C:\> Start-Container myContainer | Wait-Container
PS C:\> ConvertTo-ContainerImage -ContainerIdOrName myContainer -Repository myWorkloadImage
```

Creates a new container named "myContainer" that runs the application "myWorkload.exe", starts it, waits for it to complete, then commits the completed container as a new image named "myWorkloadImage".
## PARAMETERS

### -Author
Specifies the value for the Author metadata on the resulting image.





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

### -Configuration
An instance of a Docker.DotNet.Models.Config object with desired configuration values filled out.




```yaml
Type: Config
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Container
Specifies the container to be committed as a new image.





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

### -Message
Specifies the value for the Message metadata on the resulting image.





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

### -Repository
Specifies the Repository name to use for the new image.





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

### -Tag
Optionally specifies the Tag to use for the new image. If no Tag is supplied, defaults to "latest".





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

### -ContainerIdOrName
Specifies the container by Id or Name that will be committed into the new image.

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

### Docker.DotNet.Models.ImagesListResponse

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/ConvertTo-ContainerImage.md)






